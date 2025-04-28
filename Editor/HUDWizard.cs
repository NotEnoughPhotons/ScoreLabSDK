#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace NEP.ScoreLab.Editor
{
    public class HUDWizard : EditorWindow
    {
        private string m_hudName;
        
        [MenuItem("Not Enough Photons/ScoreLab/HUD Wizard", false, 10)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(HUDWizard));
            window.titleContent = new GUIContent("HUD Wizard");
        }

        private void Awake()
        {
        }

        private void OnGUI()
        {
            m_hudName = EditorGUILayout.TextField("Name", m_hudName);

            if (m_hudName == string.Empty)
            {
                EditorGUILayout.HelpBox("Please enter a name for the HUD, it cannot be empty!", MessageType.Error);
                return;
            }
            
            if (GUILayout.Button("Generate"))
            {
                Construct(m_hudName);
            }
        }

        private void Construct(string name)
        {
            string root = Application.dataPath;
            string uiFolder = Path.Combine(root, "Custom UIs");
            string hudFolder = Path.Combine(uiFolder, name);

            if (Directory.Exists(hudFolder))
            {
                EditorUtility.DisplayDialog("ScoreLab SDK - Error", "HUD already exists!", "OK");
                return;
            }
            
            Directory.CreateDirectory(Path.Combine(hudFolder, "Animations"));
            Directory.CreateDirectory(Path.Combine(hudFolder, "Data"));
            Directory.CreateDirectory(Path.Combine(hudFolder, "Fonts"));
            Directory.CreateDirectory(Path.Combine(hudFolder, "Sounds"));
            Directory.CreateDirectory(Path.Combine(hudFolder, "Prefabs"));
            Directory.CreateDirectory(Path.Combine(hudFolder, "Textures"));
            
            AssetDatabase.Refresh();

            string prefabName = name + ".prefab";
            string prefabPath = Path.Combine(hudFolder, prefabName);
            
            prefabPath = AssetDatabase.GenerateUniqueAssetPath(prefabPath);

            GameObject prefab = new GameObject();
            prefab.name = name;
            
            if (PrefabUtility.SaveAsPrefabAsset(prefab, prefabPath))
            {
                EditorUtility.DisplayDialog("ScoreLab SDK", "Prefab generated.", "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("ScoreLab SDK", "Could not create prefab.", "OK");
            }
            
        }
    }
}
#endif