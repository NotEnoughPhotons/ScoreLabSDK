#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
public class AssetBundleGen : MonoBehaviour
{
	[MenuItem("Assets/Build Project Bundles")]
	private static void BuildBundles()
	{
		BuildPipeline.BuildAssetBundles(Application.dataPath + "/AssetBundles/", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
	}
}
#endif