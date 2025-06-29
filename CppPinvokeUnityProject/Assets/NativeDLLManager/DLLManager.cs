using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

//1.PInvoke加载的DLL会被CLR缓存，无法卸载，除非程序关闭。导致我们调试修改DLL麻烦
//2.直接调用每个C++函数 会很慢.我们采用先加载C++的DLL，得到DLL的内存位置，然后通过它去调用C++函数，速度会快很多
//该属性是基于OS的，所以不会存在跨平台的问题。
public static class DLLManager
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


  static DLLManager()
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


#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
        //OSX 和Linux下的导入
        [DllImport(DLLName__Internal)]
        public static extern IntPtr dlopen(string path, int flag);
        [DllImport(DLLName__Internal)]
        public static extern IntPtr dlsym(IntPtr handle, string symbolName);
        [DllImport(DLLName__Internal)]
        public static extern int dlclose(IntPtr handle);

        public static IntPtr LoadLibrary(string path)
        {
            IntPtr handle = dlopen(path, 0);
            if(handle == IntPtr.Zero)
            {
               throw new Exception("Couldn't open native library: "+ path);
            }
            return handle;
        }
        public  static IntPtr GetProcAddress(IntPtr libraryHandle, string functionName)
        { 
            return DLLManager.dlsym(libraryHandle, functionName);
        }
        public static void FreeLibrary(IntPtr libraryHandle)
        {
             dlclose(libraryHandle);
        }
#else   
//#elif UNITY_EDITOR_WIN

    // win 编辑器下
    [DllImport(DLLName_kernel32)]
    public extern static IntPtr LoadLibrary(string path);
    [DllImport(DLLName_kernel32)]
    public extern static IntPtr GetProcAddress(IntPtr lib, string funcName);
    [DllImport(DLLName_kernel32)]
    public extern static bool FreeLibrary(IntPtr lib);

    public static IntPtr OpenLibrary(string fullPath)
    {
        // 1. 动态加载 DLL
        // 可以指定DLL的完整路径，或者让系统在标准搜索路径中查找
        IntPtr handle = LoadLibrary(fullPath);
        if (handle == IntPtr.Zero)
        {
            throw new Exception("Couldn't open native library: " + fullPath);
        }
        else
        {
            Debug.Log("加载native library 成功:" + fullPath);
        }
        return handle;
    }
#endif


    //无需重启Unity,只限于Win32


   //处理DLL的绝对目录：方法1
    public static void EnvironmentSetting()
    {
        string currentPath = Environment.GetEnvironmentVariable("PATH", EnvironmentVariableTarget.Process);
        string dllPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + DLLPath;
        if (!currentPath.Contains(dllPath))
        {
          //用代码将非托管DLL所在的目录添加到PATH环境变量中，
          Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator + dllPath, EnvironmentVariableTarget.Process);
        }
    }

  //处理DLL的绝对目录：方法2 使用程序集位置
    public static IntPtr LoadDLL_Win32(string dllName)
    {
        string currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        string dllPath = Path.Combine(currentDirectory, DLLPath + @"\" + dllName + ".dll");
        return LoadLibrary(dllPath);
    }


    //获取某个函数的指针
    public static IntPtr GetFuncPtrCrossPlatform(IntPtr libraryHandle, string functionName)
    {
        IntPtr symbol = GetProcAddress(libraryHandle, functionName);
        if (symbol == IntPtr.Zero)
        {
            throw new Exception("Couldn't get function:" + functionName);
        }
        else
            Debug.LogError(" get function IntPtr：" + functionName);
        return symbol;
    }


    public static T GetDelegate<T>(IntPtr libraryHandle, string functionName) where T : class
    {
        IntPtr symbol = GetFuncPtrCrossPlatform(libraryHandle, functionName);
        return Marshal.GetDelegateForFunctionPointer<T>(symbol);
    }

    //将要执行的函数转换为委托
    public static Delegate GetDelegate(IntPtr libraryHandle, string functionName, Type t)
    {
        IntPtr api = GetProcAddress(libraryHandle, functionName);
        return Marshal.GetDelegateForFunctionPointer(api, t);
    }
}
