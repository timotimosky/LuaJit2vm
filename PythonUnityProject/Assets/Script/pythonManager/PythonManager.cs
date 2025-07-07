using Python.Runtime;
using Python.Runtime.Native;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PythonManager
{
    private static string pythonDLLPath = @"\Assets\Plugins\x86_64\python312.dll";
    static string pycode_folder_path;
    static string python_install_path;


    private static string pythonHome = @"C:\Python312";
    private static string pythonPath = @"C:\Python312;C:\Python312\Lib\site-packages";
    // Python引擎初始化标志
    private static bool pythonInitialized = false;

    public static bool Init()
    {
        InitPath();

        if (!PythonEngine.IsInitialized)
        {
            PythonEngine.Initialize();
            pythonInitialized = true;
        }

        if (!PythonEngine.IsInitialized)
        {
            Debug.LogError("初始化python虚拟机失败");
            return false;
        }
        InsertPythonSearchPath();
        return true;
    }

    private static void InitPath()
    {
        if (PythonEngine.IsInitialized)
        {
            return;
        }
        UnityEngine.Debug.Log("Python engine has been initialized!");
        UnityEngine.Debug.Log(Application.persistentDataPath);

        python_install_path = Application.persistentDataPath + "/pythonhome";

        string CurrentPath = System.IO.Directory.GetCurrentDirectory();
        UnityEngine.Debug.Log(CurrentPath);
        pycode_folder_path = CurrentPath + "\\Assets\\script\\pythonScript";//存放python脚本的目录

        var lib = new[]
        {
                pycode_folder_path,
               // Path.Combine(pathToPython, "Lib"),
               // Path.Combine(pathToPython, "DLLs")
        };
        string paths = string.Join(";", lib);

        // 设置Python环境变量  初始化搜索地址,用于搜索第三方python库和脚本
        Environment.SetEnvironmentVariable("PYTHONHOME", pythonHome, EnvironmentVariableTarget.Process);
        Environment.SetEnvironmentVariable("PYTHONPATH", pythonPath);

        // 设置Python DLL路径
        Runtime.PythonDLL = CurrentPath + pythonDLLPath;
    }

    //必须初始化虚拟机之后再执行
    private static void InsertPythonSearchPath()
    {
        PyGILState gs = PythonEngine.AcquireLock();
        try
        {
            // 添加pycode的路径
            string insert_sys_path = "import sys;sys.path.insert(0, '" + pycode_folder_path + "')";
            int ret = PythonEngine.RunSimpleString(insert_sys_path);
            if (ret != 0)
            {
                Debug.LogError("GE_EXC insert_sys_path error");
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
        }
        finally
        {
            PythonEngine.ReleaseLock(gs);
        }
    }


    private static void InstallPython()
    {
        // 如果已经安装，则无需重复安装
        string python_flag_path = python_install_path + "/install_flag.txt";
        if (File.Exists(python_flag_path))
        {
            return;
        }
        // 安装python . python的exe安装程序被打包成了二进制
        TextAsset install_ta = Resources.Load<TextAsset>("Python/Install");
        // 居然每次拿bytes都需要重新加载
        byte[] install_bytes = install_ta.bytes;
        // 只支持64位，不再支持32位
        string os_name = GetPlatformString();
        string install_name = "Python/" + os_name;
        TextAsset list_ta = Resources.Load<TextAsset>(install_name);
        string[] lines = list_ta.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            string[] cells = lines[i].Split('\t');
            string relative_path = cells[0];
            int start_index = int.Parse(cells[1]);
            int length = int.Parse(cells[2]);
            // GCIO.Instance.CreateFile(this.python_install_path, relative_path, install_bytes, start_index, length);
        }
        Resources.UnloadAsset(install_ta);
        Resources.UnloadAsset(list_ta);
        // 标记安装成功
        File.Create(python_flag_path);
    }

    public static string GetPlatformString()
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            return "iPhone";
        }
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WSAPlayer)
        {
            return "Windows Store Apps";
        }
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.PS4)
        {
            return "PS4";
        }
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.XboxOne)
        {
            return "XboxOne";
        }
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.tvOS)
        {
            return "tvOS";
        }
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            return "Android";
        }
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneLinux64 ||
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows ||
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64 ||
            EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneOSX
            )
        {
            return "Standalone";
        }
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.WebGL)
        {
            return "WebGL";
        }
        return null;
    }


    public static bool IsInMainThread()
    {
        return System.Threading.Thread.CurrentThread.ManagedThreadId == Runtime.MainManagedThreadId;
    }

    public static bool CheckNoCoroutine()
    {
        var stacktrace = new StackTrace();
        for (var i = 0; i < stacktrace.FrameCount; i++)
        {
            string name = stacktrace.GetFrame(i).GetMethod().Name;
            if (name == "StartCoroutine" || name == "InvokeMoveNext")
            {
                UnityEngine.Debug.Log("GE_EXC Python Coroutine Error");
                UnityEngine.Debug.Log(stacktrace.ToString());
                return false;
            }

        }
        return true;
    }


    /// <summary>
    /// 调用一个python的模块函数
    /// </summary>
    /// <param name="module_name">模块名</param>
    /// <param name="function_name">函数名</param>
    public static void CallPythonModuleFunction(string module_name, string function_name, string s = null)
    {
        if (!PythonEngine.IsInitialized)
        {
            return;
        }
        if (!IsInMainThread())
        {
            return;
        }

        if (!CheckNoCoroutine())
        {
            return;
        }

        PyGILState gs = PythonEngine.AcquireLock();
        PyString py_str = null;
        PyObject py_module = null;
        PyObject py_result = null;

        py_module = PythonEngine.ImportModule(module_name);

        if(py_module == null) {
            UnityEngine.Debug.LogError($"[GCPython::CallPythonModuleFunction  Null,] [{module_name}]");
            return;
        }

        try
        {
            if (s != null)
            {
                py_str = new PyString(s);
                py_result = py_module.InvokeMethod(function_name, py_str);
            }
            else
                py_result = py_module.InvokeMethod(function_name);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError($"[GCPython::CallPythonModuleFunction] [{module_name}] [{function_name}] [{e.ToString()}]");
        }
        finally
        {
            if (py_str != null) { py_str.Dispose(); }
            if (py_module != null) { py_module.Dispose(); }
            if (py_result != null) { py_result.Dispose(); }
            PythonEngine.ReleaseLock(gs);
        }
    }
}
