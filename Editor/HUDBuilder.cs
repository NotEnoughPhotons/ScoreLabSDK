#if UNITY_EDITOR
using System;
using System.IO;
using NEP.ScoreLab.Data;
using NEP.ScoreLab.UI;
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

			if (!m_targetPrefab.GetComponent<UIController>())
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
			
			if (GUILayout.Button("Build"))
			{
				string hudName = m_targetManifestObject.manifest.Name;
				string exportLocation = Path.Combine(Application.dataPath, "Built HUDs");

				AssetBundleBuild[] bundles = new AssetBundleBuild[1];
				bundles[0].assetBundleName = hudName.ToLower() + ".hud";
				bundles[0].assetNames = new string[]
				{
					AssetDatabase.GetAssetPath(m_targetPrefab)
				};
				
				string buildPath = Path.Combine(exportLocation, m_targetPlatform == TargetPlatform.PCVR ? "PCVR" : "Quest");
				BuildTarget buildTarget = m_targetPlatform == TargetPlatform.PCVR ? BuildTarget.StandaloneWindows64 : BuildTarget.Android;

				Directory.CreateDirectory(buildPath);
				var bundleManifest = BuildPipeline.BuildAssetBundles(buildPath, bundles, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
				
				string manifestWritePath = Path.Combine(exportLocation, $"{hudName.ToLower()}.hud_manifest");
				StreamWriter sw = new StreamWriter(manifestWritePath);
				m_targetManifestObject.SetGUID(bundleManifest.GetAssetBundleHash(bundleManifest.GetAllAssetBundles()[0]).ToString());
				sw.Write(m_targetManifestObject.ToJSON());
				sw.Dispose();
				sw.Close();
			}
		}
	}
}
#endif