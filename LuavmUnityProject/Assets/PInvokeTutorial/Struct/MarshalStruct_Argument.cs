using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalStruct_Argument
{
    class MarshalStruct_Argument
    {

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

        // void __cdecl TestStructArgumentByVal(SIMPLESTRUCT simpleStruct);
        [DllImport(DllLoader._dllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private extern static void TestStructArgumentByVal(ManagedSimpleStruct argStruct);

        // void __cdecl TestStructArgument(PSIMPLESTRUCT pStruct);
        [DllImport(DllLoader._dllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private extern static void TestStructArgumentByRef(ref ManagedSimpleStruct argStruct);

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct ManagedSimpleStruct_WrongPack
        {
            public int intValue;
            public short shortValue;
            public float floatValue;
            public double doubleValue;
        }

        // void __cdecl TestStructArgument(PARGUMENTSTRUCT pStruct);
        [DllImport(DllLoader._dllName, CharSet = CharSet.Unicode,
            CallingConvention = CallingConvention.Cdecl,
            EntryPoint = "TestStructArgumentByRef")]
        private extern static void TestStructArgumentWrongPack(ref ManagedSimpleStruct_WrongPack argStruct);

        static void Main(string[] args)
        {
            TestStructArgumentVal();

            TestCorrectPack();

            TestWrongPack();

            UnityEngine.Debug.LogFormat("\r\n按任意键退出...");
            Console.Read();
        }

        private static void TestStructArgumentVal()
        {
            ManagedSimpleStruct simpleStruct = new ManagedSimpleStruct();
            simpleStruct.intValue = 10;
            simpleStruct.shortValue = 20;
            simpleStruct.floatValue = 3.5f;
            simpleStruct.doubleValue = 6.8f;
            //平台调用
            TestStructArgumentByVal(simpleStruct);

            UnityEngine.Debug.LogFormat("\n结构体新数据：int = {0}, short = {1}, float = {2:f6}, double = {3:f6}",
                simpleStruct.intValue, simpleStruct.shortValue, simpleStruct.floatValue, simpleStruct.doubleValue);
        }

        private static void TestCorrectPack()
        {
            UnityEngine.Debug.LogFormat("托管代码定义的结构体在非托管代码中的大小为：{0}字节", Marshal.SizeOf(typeof(ManagedSimpleStruct)));
            ManagedSimpleStruct argStruct = new ManagedSimpleStruct();
            argStruct.intValue = 1;
            argStruct.shortValue = 2;
            argStruct.floatValue = 3.0f;
            argStruct.doubleValue = 4.5f;
            TestStructArgumentByRef(ref argStruct);

            UnityEngine.Debug.LogFormat("\n结构体新数据：int = {0}, short = {1}, float = {2:f6}, double = {3:f6}",
                argStruct.intValue, argStruct.shortValue, argStruct.floatValue, argStruct.doubleValue);
        }

        private static void TestWrongPack()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            UnityEngine.Debug.LogFormat("\n========================\n错误的内存对齐方式:");
            Console.ResetColor();
            UnityEngine.Debug.LogFormat("托管代码定义的结构体在非托管代码中的大小为：{0}字节", Marshal.SizeOf(typeof(ManagedSimpleStruct_WrongPack)));
            ManagedSimpleStruct_WrongPack argStruct = new ManagedSimpleStruct_WrongPack();
            argStruct.intValue = 1;
            argStruct.shortValue = 2;
            argStruct.floatValue = 3.0f;
            argStruct.doubleValue = 4.5f;
            TestStructArgumentWrongPack(ref argStruct);

            UnityEngine.Debug.LogFormat("\n结构体新数据：int = {0}, short = {1}, float = {2:f6}, double = {3:f6}",
                argStruct.intValue, argStruct.shortValue, argStruct.floatValue, argStruct.doubleValue);
        }

    }
}
