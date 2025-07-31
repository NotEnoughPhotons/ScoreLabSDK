#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using System.IO;
using NEP.ScoreLab.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace NEP.ScoreLab.Editor
{
    public class ParExporter : EditorWindow
    {
        [SerializeField] List<ParDefinition> m_definitions = new();
        private string m_exportLocation;
        
        [MenuItem("Not Enough Photons/ScoreLab/Export Par Definitions", false, 10)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(ParExporter));
            window.titleContent = new GUIContent("Par Exporter");
        }

        private void OnGUI()
        {
            ScriptableObject target = this;
            SerializedObject serializedObject = new SerializedObject(target);
            SerializedProperty definitions = serializedObject.FindProperty("m_definitions");
            
            EditorGUILayout.PropertyField(definitions, true);
            serializedObject.ApplyModifiedProperties();
            
            if (m_definitions == null || m_definitions.Count == 0)
            {
                EditorGUILayout.HelpBox(
                    "At least one par definition is required!",
                    MessageType.Error);
                return;
            }
            
            m_exportLocation = EditorGUILayout.TextField("Export Location:", m_exportLocation);

            if (GUILayout.Button("Export"))
            {
                WriteJSONDefinition(m_definitions.ToArray());
                CleanupBuildDirectory(m_exportLocation);
            }
        }

        private void WriteJSONDefinition(ParDefinition[] definitions)
        {
            if (definitions == null || definitions.Length == 0)
            {
                return;
            }

            string exportPath = Path.Combine(m_exportLocation, "pars.json");

            Directory.CreateDirectory(m_exportLocation);

            using (StreamWriter sw = new StreamWriter(exportPath))
            {
                using (JsonTextWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    
                    writer.WriteStartObject();
                    foreach (var definition in definitions)
                    {
                        JToken token = definition.ToJson();
                        token.WriteTo(writer);
                    }
                    writer.WriteEndObject();
                }
            }
        }
        
        private void CleanupBuildDirectory(string directory)
        {
            if (directory == string.Empty)
            {
                return;
            }

            string[] files = Directory.GetFiles(directory);

            for (int i = 0; i < files.Length; i++)
            {
                string file = files[i];

                bool isBlacklisted = file.EndsWith(".manifest") || file.EndsWith(".meta");
                
                if (isBlacklisted)
                {
                    // NOTE: This is pretty dangerous. Must replace with something safer.
                    File.Delete(file);
                }
                
                // Typically when a HUD is built, there's a file without an extension.
                // This file is named after whatever the parent folder is named.
                // The file has a magic header named "UnityFS".

                DirectoryInfo parentDirectory = Directory.GetParent(file);
                
                if (parentDirectory == null)
                {
                    continue;
                }

                if (!file.EndsWith(parentDirectory.Name))
                {
                    continue;
                }
                
                bool isCorrectFile = false;
                
                using (FileStream stream = new FileStream(file, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        char[] chars = reader.ReadChars(7);
                        
                        // Have to use a string builder here, otherwise -
                        // System.Chars[] will get outputted if I try to print it normally.
                        // Stupid.
                        StringBuilder builder = new StringBuilder();
                        builder.Append(chars);

                        string magic = builder.ToString();

                        // Update the flag so we know it's the correct file
                        if (magic == "UnityFS")
                        {
                            isCorrectFile = true;
                        }
                    }
                }

                if (isCorrectFile)
                {
                    File.Delete(file);
                }
            }
        }
    }
}
#endif