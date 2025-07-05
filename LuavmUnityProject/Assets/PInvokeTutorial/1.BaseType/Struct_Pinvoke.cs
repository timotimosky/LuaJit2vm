using fts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//typedef struct ANativeStruct
//{
//    int employeeID;
//    float employedYear;
//    int intList[255]; 
//    char displayName[255]; 
//} ANativeStruct, *PtrANativeStruct;
[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
public struct ANativeStruct
{
    public int intValue;
    public float floatValue;
   // public bool c;

    //单独声明内存布局
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public int[] intList;

    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
    public string DisplayName;

    public ANativeStruct(int _intValue, float _floatValue, int[] _intlist, string _DisplayName)
    {
        intValue = _intValue;
        floatValue = _floatValue;
        intList = _intlist;
        DisplayName = _DisplayName;
    }
};


#if UseDynamicDLL
[NativeDLL(DllLoader._dllName)]
#endif
public class Struct_Pinvoke 
{

    // ------------------------------------------------------------------------
    // Example C API defined in my_cool_plugin.h
    // ------------------------------------------------------------------------
    /*
    extern "C" 
    {
        __declspec(dllexport) int simple_func();
        __declspec(dllexport) float sum(float a, float b);
        __declspec(dllexport) int string_length(const char* s);

        struct SimpleStruct {
            int a;
            float b;
            bool c;
        };
        __declspec(dllexport) double send_struct(SimpleStruct const* ss);
        __declspec(dllexport) SimpleStruct recv_struct();
    }
    */

#if UseDynamicDLL
    [NativeDLLFunction]
    public static SimpleFunc simple_func;
    public delegate int SimpleFunc();

    [NativeDLLFunction]
    public static Sum sum = null;
    public delegate float Sum(float a, float b);

    [NativeDLLFunction]
    public static StringLength string_length = null;
    public delegate int StringLength([MarshalAs(UnmanagedType.LPStr)] string s);

    [NativeDLLFunction]
    public static SendStruct send_struct = null;
    public delegate double SendStruct(ref ANativeStruct ss);

    [NativeDLLFunction]
    public static RecvStruct recv_struct = null;
    public delegate ANativeStruct RecvStruct();

#else
    [DllImport(DllLoader._dllName, EntryPoint = "simple_func")]
    extern static public int simple_func();

    [DllImport(DllLoader._dllName, EntryPoint = "sum")]
    extern static public float sum(float a, float b);

    [DllImport(DllLoader._dllName, EntryPoint = "string_length")]
    extern static public int string_length([MarshalAs(UnmanagedType.LPStr)] string s);

    [DllImport(DllLoader._dllName, EntryPoint = "send_struct")]
    extern static public double send_struct(ref ANativeStruct ss);

    [DllImport(DllLoader._dllName, EntryPoint = "recv_struct")]
    extern static public ANativeStruct recv_struct();


    [DllImport(DllLoader._dllName)]
    private static extern void SetStruct(IntPtr stu, int count);

    //在整个调用过程中，指定CharSet
    [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private extern static IntPtr GetNewStruct();

    // void __cdecl GetNativeStructRef(PtrANativeStruct pEmployee)
    [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private extern static void GetNativeStructRef(ref ANativeStruct employee);

    [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl)]
    private extern static void FreeStruct(IntPtr pStruct);

    [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl)]
    private extern static void TestReturnStructFromArg(ref IntPtr pStruct);
#endif


    private static void TestAllocString()
    {
        ANativeStruct employee = new ANativeStruct();
        employee.intValue = 1234;
        GetNativeStructRef(ref employee);

        UnityEngine.Debug.LogFormat("ANativeStruct int: {0}", employee.intValue);
        UnityEngine.Debug.LogFormat("ANativeStruct floatValue:{0}", employee.floatValue);
        UnityEngine.Debug.LogFormat("AliANativeStruct intList: {0}", employee.intList);
        UnityEngine.Debug.LogFormat("ANativeStruct DisplayName: {0}", employee.DisplayName);
    }


    private static void TestGetNewStruct()
    {
        IntPtr pStruct = GetNewStruct();
        ANativeStruct retStruct = (ANativeStruct)Marshal.PtrToStructure(pStruct, typeof(ANativeStruct));

        // 在非托管代码中使用new/malloc分配的内存，
        // 需要调用对应的释放内存的释放方法将其释放掉
        // FreeStruct(pStruct);
        // 在非托管代码中使用CoTaskMemAlloc分配的内存，可以使用Marshal.FreeCoTaskMem方法释放
        Marshal.FreeCoTaskMem(pStruct);
        UnityEngine.Debug.LogFormat("\n非托管函数返回的结构体数据 int："+
            retStruct.intValue+"  float:"+ retStruct.floatValue);

    }

    private static void TestReturnStructByCoTaskMemAlloc()
    {
        IntPtr pStruct = GetNewStruct();
        ANativeStruct retStruct = (ANativeStruct)Marshal.PtrToStructure(pStruct, typeof(ANativeStruct));

        // 在非托管代码中使用CoTaskMemAlloc分配的内存，可以使用Marshal.FreeCoTaskMem方法释放
        Marshal.FreeCoTaskMem(pStruct);

        UnityEngine.Debug.LogFormat("\n返回的结构体数据：int = {0}, short = {1}}",
            retStruct.intValue, retStruct.floatValue);
    }

    private static void TestReturnStructByArg()
    {
        IntPtr ppStruct = IntPtr.Zero;

        TestReturnStructFromArg(ref ppStruct);

        ANativeStruct retStruct = (ANativeStruct)Marshal.PtrToStructure(ppStruct, typeof(ANativeStruct));

        // 在非托管代码中使用CoTaskMemAlloc分配的内存，可以使用Marshal.FreeCoTaskMem方法释放
        Marshal.FreeCoTaskMem(ppStruct);

        UnityEngine.Debug.LogFormat("\n返回的结构体数据：int = {0}, short = {1}}",retStruct.intValue, retStruct.floatValue);
    }

    public static void Test()
    {
        //TestGetNewStruct();
        TestAllocString();
        // TestReturnStructByCoTaskMemAlloc();

        //TestReturnStructByArg();

        //// 获取结构体类型
        //ANativeStruct stu = new ANativeStruct();
        //int Managedcpp_struct_one_size = Marshal.SizeOf(typeof(ANativeStruct));
        //IntPtr Managedcpp_struct_one_buffer = Marshal.AllocHGlobal(Managedcpp_struct_one_size);

        //SetStruct(Managedcpp_struct_one_buffer, 10);
        //ANativeStruct stu3 = (ANativeStruct)Marshal.PtrToStructure(Managedcpp_struct_one_buffer, typeof(ANativeStruct));

        //Debug.LogError("cpp_get_struct_one_value " + stu.intValue + " " + stu.floatValue);

    }
}
