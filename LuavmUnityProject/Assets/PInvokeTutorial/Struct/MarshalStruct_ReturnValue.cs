using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalStruct_ReturnValue
{
    class MarshalStruct_ReturnValue
    {
        private const string _dllName = "NativeLib.dll";

        // 结构体示例函数导入
        //typedef struct _SIMPLESTRUCT
        //{
        //    int    intValue;
        //    short  shortValue;
        //    float  floatValue;
        //    double doubleValue;
        //} SIMPLESTRUCT, *PSIMPLESTRUCT;
        [StructLayout(LayoutKind.Sequential)]
        private struct ManagedSimpleStruct
        {
            public int intValue;
            public short shortValue;
            public float floatValue;
            public double doubleValue;
        }

        // PSIMPLESTRUCT __cdecl TestStructAsResult(void);
        [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static IntPtr TestReturnStruct();

        // PSIMPLESTRUCT __cdecl TestReturnNewStruct(void)
        [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static IntPtr TestReturnNewStruct();

        // void __cdecl FreeStruct(PSIMPLESTRUCT pStruct)
        [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static void FreeStruct(IntPtr pStruct);

        // void __cdecl TestReturnStructFromArg(PSIMPLESTRUCT* ppStruct)
        [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static void TestReturnStructFromArg(ref IntPtr pStruct);

        static void Main(string[] args)
        {
            TestReturnStructByNew();

            TestReturnStructByCoTaskMemAlloc();

            TestReturnStructByArg();

            UnityEngine.Debug.LogFormat("\r\n按任意键退出...");
            Console.Read();
        }

        private static void TestReturnStructByNew()
        {
            IntPtr pStruct = TestReturnNewStruct();
            ManagedSimpleStruct retStruct =
                (ManagedSimpleStruct)Marshal.PtrToStructure(pStruct, typeof(ManagedSimpleStruct));

            // 在非托管代码中使用new/malloc分配的内存，
            // 需要调用对应的释放内存的释放方法将其释放掉
            FreeStruct(pStruct);

            UnityEngine.Debug.LogFormat("\n非托管函数返回的结构体数据：int = {0}, short = {1}, float = {2:f6}, double = {3:f6}",
                retStruct.intValue, retStruct.shortValue, retStruct.floatValue, retStruct.doubleValue);
 
        }

        private static void TestReturnStructByCoTaskMemAlloc()
        {
            IntPtr pStruct = TestReturnStruct();
            ManagedSimpleStruct retStruct =
                (ManagedSimpleStruct)Marshal.PtrToStructure(pStruct, typeof(ManagedSimpleStruct));

            // 在非托管代码中使用CoTaskMemAlloc分配的内存，可以使用Marshal.FreeCoTaskMem方法释放
            Marshal.FreeCoTaskMem(pStruct);

            UnityEngine.Debug.LogFormat("\n返回的结构体数据：int = {0}, short = {1}, float = {2:f6}, double = {3:f6}",
                retStruct.intValue, retStruct.shortValue, retStruct.floatValue, retStruct.doubleValue);
        }

        private static void TestReturnStructByArg()
        {
            IntPtr ppStruct = IntPtr.Zero;

            TestReturnStructFromArg(ref ppStruct);

            ManagedSimpleStruct retStruct =
                (ManagedSimpleStruct)Marshal.PtrToStructure(ppStruct, typeof(ManagedSimpleStruct));

            // 在非托管代码中使用CoTaskMemAlloc分配的内存，可以使用Marshal.FreeCoTaskMem方法释放
            Marshal.FreeCoTaskMem(ppStruct);

            UnityEngine.Debug.LogFormat("\n返回的结构体数据：int = {0}, short = {1}, float = {2:f6}, double = {3:f6}",
                retStruct.intValue, retStruct.shortValue, retStruct.floatValue, retStruct.doubleValue);

        }
    }
}
