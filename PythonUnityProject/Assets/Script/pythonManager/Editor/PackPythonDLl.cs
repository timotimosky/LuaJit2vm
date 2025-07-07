using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PackPythonDLl : MonoBehaviour
{
    [MenuItem("Game/PackPythonDLL", priority = 201)]
    private static void PackPyton()
    {
        BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
        string dst_dir = $"Assets/Local/HybridCLRRuntime/{target}/Entry";
        string ad_asset = "python312";

        //string src_dll = $"{settings.hotUpdateDllCompileOutputRootDir}/{target}/{ad_asset}.dll";
        //string dst_dll = $"{dst_dir}/{ad_asset}.dll.bytes";
        //File.Copy(src_dll, dst_dll, true);
        //string src_pdb = $"{settings.hotUpdateDllCompileOutputRootDir}/{target}/{ad_asset}.pdb";
        //string dst_pdb = $"{dst_dir}/{ad_asset}.pdb.bytes";
        //File.Copy(src_pdb, dst_pdb, true);

        // 先刷新一次资源，确保后面的绑定能找到资源
      //  AssetHelp.SaveAndRefresh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
