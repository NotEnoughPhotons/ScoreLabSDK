#if UNITY_EDITOR
using System;
using System.IO;
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

		private string m_exportLocation;
		private TargetPlatform m_targetPlatform;
		private GameObject m_targetPrefab;
	
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
			
			if (GUILayout.Button("Build"))
			{
				if (!m_targetPrefab)
				{
					throw new NullReferenceException("Missing prefab!");
				}

				AssetBundleBuild[] bundles = new AssetBundleBuild[1];
				bundles[0].assetBundleName = m_targetPrefab.name + ".hud";
				string[] assets = new string[1];
				assets[0] = AssetDatabase.GetAssetPath(m_targetPrefab);
				bundles[0].assetNames = assets;
			
				string buildPath = Path.Combine(m_exportLocation, m_targetPlatform == TargetPlatform.PCVR ? "PCVR" : "Quest");
				BuildTarget buildTarget = m_targetPlatform == TargetPlatform.PCVR ? BuildTarget.StandaloneWindows64 : BuildTarget.Android;
			
				Directory.CreateDirectory(buildPath);
				BuildPipeline.BuildAssetBundles(buildPath, bundles, BuildAssetBundleOptions.ChunkBasedCompression, buildTarget);
			}
		}
	}
}
#endif