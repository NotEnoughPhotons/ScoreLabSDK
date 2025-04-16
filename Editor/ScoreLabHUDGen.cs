#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class ScoreLabHUDGen : EditorWindow
{
	private enum TargetPlatform
	{
		PCVR,
		Quest
	}

	private string m_exportLocation;
	private TargetPlatform m_targetPlatform;
	private GameObject m_targetPrefab;
	
	[MenuItem("Not Enough Photons/ScoreLab/Build")]
	public static void ShowWindow()
	{
		GetWindow(typeof(ScoreLabHUDGen));
	}

	private void OnGUI()
	{
		m_targetPlatform = (TargetPlatform)EditorGUILayout.EnumPopup("Platform:", m_targetPlatform);

		m_targetPrefab = EditorGUILayout.ObjectField(m_targetPrefab, typeof(GameObject), false) as GameObject;
		
		m_exportLocation = EditorGUILayout.TextField("Export Location", m_exportLocation);
		
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
#endif