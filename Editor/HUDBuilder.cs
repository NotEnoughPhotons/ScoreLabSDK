#if UNITY_EDITOR
using System.IO;
using NEP.ScoreLab.Data;
using NEP.ScoreLab.HUD;
using UnityEngine;
using UnityEditor;

namespace NEP.ScoreLab.Editor
{
    public class HUDBuilder : EditorWindow
    {
        private enum TargetPlatform
        {
            PCVR,
            Quest
        }

        private TargetPlatform m_targetPlatform;
        private GameObject m_targetPrefab;
        private HUDManifestObject m_targetManifestObject;
        private AudioManifestObject m_targetAudioManifestObject;
        private string m_exportLocation;

        [MenuItem("Not Enough Photons/ScoreLab/Build", false, 10)]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow(typeof(HUDBuilder));
            window.titleContent = new GUIContent("HUD Builder");
        }

        private void OnGUI()
        {
            m_targetPlatform = (TargetPlatform)EditorGUILayout.EnumPopup("Platform:", m_targetPlatform);

            m_targetPrefab =
                (GameObject)EditorGUILayout.ObjectField("Prefab:", m_targetPrefab, typeof(GameObject), false);

            if (!m_targetPrefab)
            {
                return;
            }

            if (!m_targetPrefab.GetComponent<ScoreLab.HUD.HUD>())
            {
                EditorGUILayout.HelpBox("A ScoreLab prefab is required to have a UIController component!",
                    MessageType.Error);
                return;
            }

            m_targetManifestObject = (HUDManifestObject)EditorGUILayout.ObjectField("Manifest:", m_targetManifestObject,
                typeof(HUDManifestObject), false);

            if (!m_targetManifestObject)
            {
                EditorGUILayout.HelpBox(
                    "A HUD manifest is required! Create one by right-clicking in the Explorer and going to Not Enough Photons/ScoreLab/HUD Manifest!",
                    MessageType.Error);
                return;
            }

            m_targetAudioManifestObject = (AudioManifestObject)EditorGUILayout.ObjectField("Audio Manifest:", m_targetAudioManifestObject,
                typeof(AudioManifestObject), false);
            
            m_exportLocation = EditorGUILayout.TextField("Export Location:", m_exportLocation);

            if (GUILayout.Button("Build"))
            {
                string exportedPath = GetExportPath();

                AssetBundleBuild[] hudBundleBuild = CreateHUDBundleBuild();
                AssetBundleBuild[] hudAudioBundleBuild = CreateHUDAudioBundleBuild();
                
                Directory.CreateDirectory(exportedPath);

                GenerateBundles(exportedPath, ".hud", hudBundleBuild);
                GenerateBundles(exportedPath, ".hud_audio", hudAudioBundleBuild);

                WriteHUDManifest(exportedPath, m_targetManifestObject.manifest.Name.ToLower());

                if (m_targetAudioManifestObject != null)
                {
                    WriteHUDAudioManifest(exportedPath, m_targetManifestObject.manifest.Name.ToLower());
                }
            }
        }

        private string GetExportPath()
        {
            string editorExportLocation = Path.Combine(Application.dataPath, "Built HUDs");
            string buildPath = "";

            if (m_exportLocation == string.Empty)
            {
                buildPath = Path.Combine(editorExportLocation,
                    m_targetPlatform == TargetPlatform.PCVR ? "PCVR" : "Quest");
            }
            else
            {
                buildPath = m_exportLocation;
            }

            return Path.Combine(buildPath, m_targetManifestObject.manifest.Name);;
        }

        private AssetBundleBuild[] CreateHUDBundleBuild()
        {
            AssetBundleBuild[] bundles = new AssetBundleBuild[1];
            bundles[0].assetBundleName = m_targetManifestObject.manifest.Name + ".hud";
            bundles[0].assetNames = new string[]
            {
                AssetDatabase.GetAssetPath(m_targetPrefab),
                AssetDatabase.GetAssetPath(m_targetManifestObject.manifest.Logo)
            };

            return bundles;
        }

        private AssetBundleBuild[] CreateHUDAudioBundleBuild()
        {
            if (m_targetAudioManifestObject == null)
            {
                return null;
            }

            int numClips = m_targetAudioManifestObject.manifest.Clips.Length;
            
            AssetBundleBuild[] bundles = new AssetBundleBuild[1];
            bundles[0].assetBundleName = m_targetManifestObject.manifest.Name + ".hud_audio";
            bundles[0].assetNames = new string[numClips];

            for (int i = 0; i < numClips; i++)
            {
                bundles[i].assetNames[i] = AssetDatabase.GetAssetPath(m_targetAudioManifestObject.manifest.Clips[i]);
            }
            
            return bundles;
        }

        private void GenerateBundles(string exportPath, string extension, AssetBundleBuild[] bundles)
        {
            BuildTarget buildTarget = m_targetPlatform == TargetPlatform.PCVR
                ? BuildTarget.StandaloneWindows64
                : BuildTarget.Android;

            BuildPipeline.BuildAssetBundles(exportPath, bundles, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);

            foreach (var file in Directory.EnumerateFiles(exportPath))
            {
                if (file.EndsWith(extension))
                {
                    continue;
                }

                File.Delete(file);
            }
        }
        
        private void WriteHUDManifest(string path, string name)
        {
            string manifestWritePath = Path.Combine(path, $"{name}.hud_manifest");
            StreamWriter hudManifestWriter = new StreamWriter(manifestWritePath);
            hudManifestWriter.Write(m_targetManifestObject.ToJSON());
            hudManifestWriter.Dispose();
            hudManifestWriter.Close();
        }

        private void WriteHUDAudioManifest(string path, string name)
        {
            string audioManifestWritePath = Path.Combine(path, $"{name}.hud_audio_manifest");
            StreamWriter audioManifestWriter = new StreamWriter(audioManifestWritePath);
            audioManifestWriter.Write(m_targetAudioManifestObject.ToJSON());
            audioManifestWriter.Dispose();
            audioManifestWriter.Close();
        }
    }
}
#endif