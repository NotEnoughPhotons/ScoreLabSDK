#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
public class AssetBundleGen : MonoBehaviour
{
	static readonly string Path_Data = Application.dataPath + "/AssetBundles/";
	static readonly string Path_PCVR = Path_Data + "PCVR";
	static readonly string Path_Quest = Path_Data + "Quest";

	[MenuItem("Not Enough Photons/Build Project Bundles (PCVR)", false, 0)]
	private static void BuildBundlesPCVR()
	{
		Directory.CreateDirectory(Path_Data);
		Directory.CreateDirectory(Path_PCVR);

		BuildPipeline.BuildAssetBundles(Path_PCVR, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows64);
	}

	[MenuItem("Not Enough Photons/Build Project Bundles (Quest)", false, 0)]
	private static void BuildBundlesQuest()
	{
		Directory.CreateDirectory(Path_Data);
		Directory.CreateDirectory(Path_Quest);

		BuildPipeline.BuildAssetBundles(Path_Quest, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
	}
}
#endif