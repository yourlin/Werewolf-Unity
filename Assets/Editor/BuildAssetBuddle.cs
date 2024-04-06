using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BuildAssetBuddle : Editor
{
    [MenuItem("linye/BuildAssetsBundle")]
    static  void BuildAssetsBundle()
    {
        #if PLATFORM_STANDALONE_WIN
            BuildPipeline.BuildAssetBundles("./AssetsBundles/win64", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        #endif
    }
}
