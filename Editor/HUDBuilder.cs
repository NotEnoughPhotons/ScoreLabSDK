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
			
			m_targetPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab:", m_targetPrefab, typeof(GameObject), false);

			if (!m_targetPrefab)
			{
				return;
			}

			if (!m_targetPrefab.GetComponent<ScoreLab.HUD.HUD>())
			{
				EditorGUILayout.HelpBox("A ScoreLab prefab is required to have a UIController component!", MessageType.Error);
				return;
			}
			
			m_targetManifestObject = (HUDManifestObject)EditorGUILayout.ObjectField("Manifest:", m_targetManifestObject, typeof(HUDManifestObject), false);

			if (!m_targetManifestObject)
			{
				EditorGUILayout.HelpBox("A HUD manifest is required! Create one by right-clicking in the Explorer and going to Not Enough Photons/ScoreLab/HUD Manifest!", MessageType.Error);
				return;
			}
			
			m_exportLocation = EditorGUILayout.TextField("Export Location:", m_exportLocation);
			
			if (GUILayout.Button("Build"))
			{
				string hudName = m_targetManifestObject.manifest.Name;
				
				AssetBundleBuild[] bundles = new AssetBundleBuild[1];
				bundles[0].assetBundleName = hudName + ".hud";
				bundles[0].assetNames = new string[]
				{
					AssetDatabase.GetAssetPath(m_targetPrefab),
					AssetDatabase.GetAssetPath(m_targetManifestObject.manifest.Logo)
				};

				if (m_exportLocation == string.Empty)
				{
					string editorExportLocation = Path.Combine(Application.dataPath, "Built HUDs");
					string buildPath = Path.Combine(editorExportLocation,
						m_targetPlatform == TargetPlatform.PCVR ? "PCVR" : "Quest");
					
					BuildTarget buildTarget = m_targetPlatform == TargetPlatform.PCVR
						? BuildTarget.StandaloneWindows64
						: BuildTarget.Android;

					string exportedPath = Path.Combine(buildPath, m_targetManifestObject.manifest.Name);

					Directory.CreateDirectory(exportedPath);
					var bundleManifest = BuildPipeline.BuildAssetBundles(exportedPath, bundles,
						BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);

					foreach (var file in Directory.EnumerateFiles(exportedPath))
					{
						if (file.EndsWith(".hud"))
						{
							continue;
						}

						File.Delete(file);
					}

					string manifestWritePath = Path.Combine(exportedPath, $"{hudName.ToLower()}.hud_manifest");
					StreamWriter sw = new StreamWriter(manifestWritePath);
					m_targetManifestObject.SetGUID(bundleManifest
						.GetAssetBundleHash(bundleManifest.GetAllAssetBundles()[0]).ToString());
					sw.Write(m_targetManifestObject.ToJSON());
					sw.Dispose();
					sw.Close();
				}
				else
				{
					string buildPath = m_exportLocation;
					
					BuildTarget buildTarget = m_targetPlatform == TargetPlatform.PCVR
						? BuildTarget.StandaloneWindows64
						: BuildTarget.Android;

					string exportedPath = Path.Combine(buildPath, m_targetManifestObject.manifest.Name);

					Directory.CreateDirectory(exportedPath);
					var bundleManifest = BuildPipeline.BuildAssetBundles(exportedPath, bundles,
						BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);

					foreach (var file in Directory.EnumerateFiles(exportedPath))
					{
						if (file.EndsWith(".hud"))
						{
							continue;
						}

						File.Delete(file);
					}

					string manifestWritePath = Path.Combine(exportedPath, $"{hudName.ToLower()}.hud_manifest");
					StreamWriter sw = new StreamWriter(manifestWritePath);
					m_targetManifestObject.SetGUID(bundleManifest
						.GetAssetBundleHash(bundleManifest.GetAllAssetBundles()[0]).ToString());
					sw.Write(m_targetManifestObject.ToJSON());
					sw.Dispose();
					sw.Close();
				}
			}
		}
	}
}
#endif