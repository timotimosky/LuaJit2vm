using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Struct_Pinvoke 
{
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
        public  int intValue;
        public float floatValue;

        //单独声明内存布局
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public int[] intList;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 255)]
        public string DisplayName;
    };

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
