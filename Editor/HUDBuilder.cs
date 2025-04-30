#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using UnityEditor;

using NEP.ScoreLab.Data;

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
        
        private readonly string[] m_whitelistedExtensions = new string[] { ".hud", ".hud_audio" };

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

                AssetBundleBuild hudBundleBuild = CreateHUDBundleBuild();
                
                Directory.CreateDirectory(exportedPath);

                GenerateBundles(exportedPath, ".hud", hudBundleBuild);

                WriteHUDManifest(exportedPath, m_targetManifestObject.manifest.Name.ToLower());

                WriteAllJSONScores(m_targetManifestObject.manifest);
                WriteAllJSONMults(m_targetManifestObject.manifest);
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

            return Path.Combine(buildPath, m_targetManifestObject.manifest.Name);
        }

        private AssetBundleBuild CreateHUDBundleBuild()
        {
            List<string> assetNames = new List<string>();
            AssetBundleBuild hudBuild = new AssetBundleBuild();
            hudBuild.assetBundleName = m_targetManifestObject.manifest.Name + ".hud";
            
            assetNames.Add(AssetDatabase.GetAssetPath(m_targetPrefab));
            assetNames.Add(AssetDatabase.GetAssetPath(m_targetManifestObject.manifest.Logo));

            if (m_targetAudioManifestObject == null)
            {
                hudBuild.assetNames = assetNames.ToArray();
                return hudBuild;
            }

            int numClips = m_targetAudioManifestObject.manifest.Clips.Length;

            for (int i = 0; i < numClips; i++)
            {
                AudioClip clip = m_targetAudioManifestObject.manifest.Clips[i];
                assetNames.Add(AssetDatabase.GetAssetPath(clip));
            }
            
            hudBuild.assetNames = assetNames.ToArray();
            
            return hudBuild;
        }

        private void GenerateBundles(string exportPath, string extension, AssetBundleBuild build)
        {
            BuildTarget buildTarget = m_targetPlatform == TargetPlatform.PCVR
                ? BuildTarget.StandaloneWindows64
                : BuildTarget.Android;

            BuildPipeline.BuildAssetBundles(exportPath, new AssetBundleBuild[1] { build }, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
        }

        private void RemoveOtherFiles(string path)
        {
            foreach (var file in Directory.EnumerateFiles(path))
            {
                foreach (var extension in m_whitelistedExtensions)
                {
                    if (!file.EndsWith(extension))
                    {
                        File.Delete(file);
                    }
                }
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

        private void WriteAllJSONScores(JSONHUDManifest manifest)
        {
            if (manifest.ScoreObjects == null || manifest.ScoreObjects.Length == 0)
            {
                return;
            }

            string exportPath = Path.Combine(m_exportLocation, manifest.Name);
            string dataPath = Path.Combine(exportPath, "Data/Score");

            Directory.CreateDirectory(dataPath);

            foreach (var scoreObject in manifest.ScoreObjects)
            {
                StreamWriter writer = new StreamWriter(dataPath + $"/{scoreObject.name}.json");
                writer.Write(scoreObject.score.ToJSON());
                writer.Dispose();
                writer.Close();
            }
        }
        
        private void WriteAllJSONMults(JSONHUDManifest manifest)
        {
            if (manifest.MultObjects == null || manifest.MultObjects.Length == 0)
            {
                return;
            }

            string exportPath = Path.Combine(m_exportLocation, manifest.Name);
            string dataPath = Path.Combine(exportPath, "Data/Multiplier");

            Directory.CreateDirectory(dataPath);
            
            foreach (var multObject in manifest.MultObjects)
            {
                StreamWriter writer = new StreamWriter(dataPath + $"/{multObject.name}.json");
                writer.Write(multObject.multiplier.ToJSON());
                writer.Dispose();
                writer.Close();
            }
        }
    }
}
#endif