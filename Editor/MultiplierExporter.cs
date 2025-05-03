#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

using Newtonsoft.Json;

using NEP.ScoreLab.Data;
using EventType = NEP.ScoreLab.Data.EventType;

namespace NEP.ScoreLab.Editor
{
    public class MultiplierExporter : EditorWindow
    {
        private MultObject m_multObject;
        private string fileName;
        
        [MenuItem("Not Enough Photons/ScoreLab/Multiplier Editor", false)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(MultiplierExporter));
            window.titleContent = new GUIContent("Multiplier Editor");
        }

        private void OnGUI()
        {
            GUILayout.Label("Fields");
            fileName = EditorGUILayout.TextField("File Name", fileName);
            m_multObject = EditorGUILayout.ObjectField(m_multObject, typeof(MultObject), false) as MultObject;

            if (m_multObject)
            {
                if (GUILayout.Button("Export"))
                {
                    string path = $"Data/Not Enough Photons/ScoreLab/Data/Multiplier/{fileName}.json";
                    using (StreamWriter sw = new StreamWriter(Path.Combine(Application.dataPath, path)))
                    {
                        using (JsonWriter writer = new JsonTextWriter(sw))
                        {
                            JSONMult mult = m_multObject.multiplier;
                            writer.Formatting = Formatting.Indented;
                            
                            writer.WriteStartObject();
                            WriteJSONMult(writer, mult);

                            if (mult.Tiers != null && mult.Tiers.Length > 0)
                            {
                                writer.WritePropertyName("Tiers");
                                writer.WriteStartArray();
                                foreach (var tier in mult.Tiers)
                                {
                                    writer.WriteStartObject();
                                    WriteJSONMult(writer, tier);
                                    writer.WriteEndObject();
                                }
                                writer.WriteEndArray();
                            }
                            
                            writer.WriteEndObject();
                        }
                    }
                }
            }
        }

        private void WriteJSONMult(JsonWriter writer, JSONMult mult)
        {
            if (mult.Name != string.Empty)
            {
                writer.WritePropertyName("Name");
                writer.WriteValue(mult.Name);
            }
            
            writer.WritePropertyName("Multiplier");
            writer.WriteValue(mult.Multiplier);
            
            
            writer.WritePropertyName("EventAudio");
            writer.WriteStartObject();
            writer.WritePropertyName("Clips");
            writer.WriteStartArray();
            for (int i = 0; i < mult.EventAudio.sounds.Length; i++)
            {
                writer.WriteValue(mult.EventAudio.sounds[i]);
            }
            writer.WriteEndArray();
            writer.WritePropertyName("Volume");
            writer.WriteValue(mult.EventAudio.volume);
            writer.WritePropertyName("Pitch");
            writer.WriteValue(mult.EventAudio.pitch);
            writer.WriteEndObject();
            
            writer.WritePropertyName("Stackable");
            writer.WriteValue(mult.Stackable);
            writer.WritePropertyName("DecayTime");
            writer.WriteValue(mult.DecayTime);

            writer.WritePropertyName("EventType");
            writer.WriteValue(EventType.MultiplierEventTable[(int)mult.EventType]);

            if (mult.Condition != string.Empty)
            {
                writer.WritePropertyName("Condition");
                writer.WriteValue(mult.Condition);
            }
            
            writer.WritePropertyName("TierRequirement");
            writer.WriteValue(mult.TierRequirement);
        }
    }
}
#endif