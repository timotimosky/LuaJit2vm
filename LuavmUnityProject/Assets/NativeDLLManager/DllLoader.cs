using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

//1.实现跨平台的动态库加载
//2.实现动态库的路径查询
//3.实现库的动态加载：
//  a.PInvoke加载的DLL会被CLR缓存，无法卸载，除非程序关闭。导致我们调试修改DLL麻烦
//  b.直接调用每个C++函数 会很慢.我们采用先加载C++的DLL，得到DLL的内存位置，然后通过它去调用C++函数，速度会快很多
public static class DllLoader
{
    public const string DLLName_kernel32 = "kernel32";//"kernel32.dll";

    // On iOS plugins are statically linked into the executable, so we have to use __Internal as the library name.
    // Other platforms load plugins dynamically, so pass the name of the plugin's dynamic library.
    public const string DLLName__Internal = "__Internal";//"__Internal.dll";


  #region delete

  public delegate void Delegate_void();
  public delegate bool Delegate_bool();
  public delegate IntPtr Delegate_IntPtr();
  public delegate uint Delegate_uint();


  public delegate void Delegate_void_IntPtr(IntPtr rb);

  #endregion


  static DllLoader()
    {
      EnvironmentSetting();
    }

#if UNITY_IPHONE
    public  const string _dllName = DLLName__Internal;
#elif UNITY_EDITOR_OSX
    public  const string _dllName = "/Cpp2Unity.bundle/Contents/MacOS/Cpp2Unity";
#elif UNITY_EDITOR_LINUX
    public  const string _dllName = "/Cpp2Unity.so";
#elif UNITY_STANDALONE_WIN
    public const string _dllName = "Cpp2Unity";
#endif

    public const string DLLPath=  "Assets/Plugins/x86_64";


#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX|| UNITY_STANDALONE_OSX

        public const string LIB_EXT = ".dylib"; 

        //OSX 和Linux下的导入
        [DllImport(DLLName__Internal)]
    private static extern IntPtr dlopen(string path, int flag);
        [DllImport(DLLName__Internal)]
    private static extern IntPtr dlsym(IntPtr handle, string symbolName);
        [DllImport(DLLName__Internal)]
        private static extern int dlclose(IntPtr handle);

        [DllImport(DLLName__Internal)]
        private static extern IntPtr dlerror();

        public static IntPtr LoadDll(string path)
        {
            const int RTLD_NOW = 2;
            IntPtr handle = dlopen(path, RTLD_NOW);
            var errPtr = dlerror();
            if (errPtr != IntPtr.Zero)
            {
                Debug.LogError(Marshal.PtrToStringAnsi(errPtr));
                throw new Exception("Couldn't open native library: " + path);
            }
            // IntPtr handle = dlopen(path, 0);
            if (handle == IntPtr.Zero)
            {
               throw new Exception("Couldn't open native library: "+ path);
            }
            Debug.Log($"Loaded {path}");
            return handle;
        }
        public  static IntPtr GetProcAddress(IntPtr libraryHandle, string functionName)
        {
            IntPtr fnPtr = dlsym(libraryHandle, functionName);
            var errPtr = dlerror();
            if (errPtr != IntPtr.Zero)
            {
                Debug.LogError(Marshal.PtrToStringAnsi(errPtr));
            }
            else
            {
                Debug.Log($"Found symbol {functionName}");
            }

            return fnPtr;
        }
        public static void FreeLibrary(IntPtr libraryHandle)
        {
            if (libraryHandle == IntPtr.Zero)
                return;
            dlclose(libraryHandle);
        }
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN

    // win 编辑器下
    [DllImport(DLLName_kernel32, SetLastError = true, CharSet = CharSet.Unicode)]
    private extern static IntPtr LoadLibrary(string path);
    [DllImport(DLLName_kernel32, SetLastError = true)]
    private extern static IntPtr GetProcAddress(IntPtr lib, string funcName);

    [DllImport(DLLName_kernel32, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private extern static bool FreeLibrary(IntPtr lib);

    [DllImport("kernel32.dll")]
    static private extern uint GetLastError();


    public const string LIB_EXT = ".dll";



    //处理DLL的绝对目录：方法2 使用程序集位置
    public static IntPtr LoadDLL_Win32(string dllName)
    {
        string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string dllPath = Path.Combine(currentDirectory, DLLPath + @"\" + dllName + LIB_EXT);
        return LoadLibrary(dllPath);
    }

    public static void FreeLib(IntPtr hLib)
    {
        if (hLib == IntPtr.Zero)
            return;

        FreeLibrary(hLib);
    }




    //处理DLL的绝对目录：方法1
    public static void EnvironmentSetting()
    {
        string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
        string dllPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + DLLPath;
        //string dllPath = Application.dataPath + "\\Plugins\\x86_64\\";


        if (!currentPath.Contains(dllPath))
        {
            //用代码将非托管DLL所在的目录添加到PATH环境变量中，
            Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath, EnvironmentVariableTarget.Process);
        }
    }

    public static IntPtr LoadDll(string fullPath)
    {
        if (!fullPath.EndsWith(DllLoader.LIB_EXT))
        {
            fullPath = fullPath+DllLoader.LIB_EXT;
        }

        //处理路径
        if (!File.Exists(fullPath))
        {
            fullPath = Application.dataPath + "\\Plugins\\x86_64\\" + fullPath;
        }


        // 1. 动态加载 DLL
        // 可以指定DLL的完整路径，或者让系统在标准搜索路径中查找
        IntPtr handle = LoadLibrary(fullPath);
        var errID = GetLastError();

        if (handle == IntPtr.Zero)
        {
            Debug.LogError(string.Format("Failed to load library [{0}]. Err: [{1}]", fullPath, errID));
            throw new Exception("Couldn't open native library: " + fullPath);
        }
        else
        {
            Debug.Log("加载native library 成功:" + fullPath);
        }
        return handle;
    }

#else
    #error Unsupported platform.
#endif

    //无需重启Unity,只限于Win32

    //获取某个函数的指针
    public static IntPtr GetFuncPtrCrossPlatform(IntPtr libraryHandle, string functionName)
    {
        IntPtr symbol = GetProcAddress(libraryHandle, functionName);
        var errID = GetLastError();
        if (symbol == IntPtr.Zero)
        {
            Debug.LogError(string.Format("Failed to find function [{0}] . Err: [{1}]", functionName, errID));
        }
        else
            Debug.Log(" get function IntPtr：" + functionName);
        return symbol;
    }


    public static T GetDelegate<T>(IntPtr libraryHandle, string functionName) where T : class
    {
        IntPtr symbol = GetFuncPtrCrossPlatform(libraryHandle, functionName);
        if (symbol == IntPtr.Zero)
            return null;
        // Get delegate pointer
        return Marshal.GetDelegateForFunctionPointer<T>(symbol);
    }

    //将要执行的函数转换为委托
    public static Delegate GetDelegate(IntPtr libraryHandle, string functionName, Type t)
    {
        IntPtr api = GetProcAddress(libraryHandle, functionName);
        return Marshal.GetDelegateForFunctionPointer(api, t);
    }
}
