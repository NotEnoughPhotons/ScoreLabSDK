#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
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
            Quest,
            Both
        }

        private TargetPlatform m_targetPlatforms;
        private GameObject m_targetPrefab;
        private HUDManifestObject m_targetManifestObject;
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
            m_targetPlatforms = (TargetPlatform)EditorGUILayout.EnumPopup("Platforms:", m_targetPlatforms);

            m_targetPrefab =
                (GameObject)EditorGUILayout.ObjectField("Prefab:", m_targetPrefab, typeof(GameObject), false);

            if (!m_targetPrefab)
            {
                return;
            }

            if (!m_targetPrefab.GetComponent<ScoreLab.HUD.HUD>())
            {
                EditorGUILayout.HelpBox("A ScoreLab prefab is required to have a HUD component!",
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
            
            m_exportLocation = EditorGUILayout.TextField("Export Location:", m_exportLocation);

            if (GUILayout.Button("Build"))
            {
                string exportedPath = GetExportPath();
                
                if (m_targetPlatforms == TargetPlatform.Both)
                {
                    BuildHUD(exportedPath, TargetPlatform.PCVR);
                    BuildHUD(exportedPath, TargetPlatform.Quest);
                }
                else
                {
                    BuildHUD(exportedPath, m_targetPlatforms);
                }
                
                WriteHUDManifest(exportedPath, m_targetManifestObject.manifest.Name.ToLower());

                WriteAllJSONScores(m_targetManifestObject.manifest);
                WriteAllJSONMults(m_targetManifestObject.manifest);
            
                CleanupBuildDirectory(exportedPath);
            }
        }

        private void BuildHUD(string exportedPath, TargetPlatform platform)
        {
            AssetBundleBuild hudBundleBuild = CreateHUDBundleBuild(platform);
            Directory.CreateDirectory(exportedPath);
            GenerateBundles(exportedPath, hudBundleBuild);
        }
        
        private string GetExportPath()
        {
            string editorExportLocation = Path.Combine(Application.dataPath, "Built HUDs");
            string buildPath = "";

            if (m_exportLocation == string.Empty)
            {
                buildPath = editorExportLocation;
            }
            else
            {
                buildPath = m_exportLocation;
            }

            return Path.Combine(buildPath, m_targetManifestObject.manifest.Name);
        }

        private AssetBundleBuild CreateHUDBundleBuild(TargetPlatform target)
        {
            List<string> assetNames = new List<string>();
            AssetBundleBuild hudBuild = new AssetBundleBuild();

            if (target == TargetPlatform.PCVR)
            {
                hudBuild.assetBundleName = m_targetManifestObject.manifest.Name + "_pcvr.hud";
            }
            else if (target == TargetPlatform.Quest)
            {
                hudBuild.assetBundleName = m_targetManifestObject.manifest.Name + "_quest.hud";
            }
            
            assetNames.Add(AssetDatabase.GetAssetPath(m_targetPrefab));
            assetNames.Add(AssetDatabase.GetAssetPath(m_targetManifestObject.manifest.Logo));

            AudioManifestObject audio = m_targetManifestObject.manifest.AudioManifest;
            
            if (audio == null)
            {
                hudBuild.assetNames = assetNames.ToArray();
                return hudBuild;
            }

            int numClips = audio.manifest.Clips.Length;

            for (int i = 0; i < numClips; i++)
            {
                AudioClip clip = audio.manifest.Clips[i];
                assetNames.Add(AssetDatabase.GetAssetPath(clip));
            }
            
            hudBuild.assetNames = assetNames.ToArray();
            
            return hudBuild;
        }

        private void GenerateBundles(string exportPath, AssetBundleBuild build)
        {
            BuildTarget buildTarget = m_targetPlatforms == TargetPlatform.PCVR
                ? BuildTarget.StandaloneWindows64
                : BuildTarget.Android;

            BuildPipeline.BuildAssetBundles(exportPath, new AssetBundleBuild[1] { build }, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
        }
        
        private void WriteHUDManifest(string path, string name)
        {
            m_targetManifestObject.manifest.AssetName = m_targetPrefab.name;
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