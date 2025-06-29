using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalString
{
    class MarshalString
    {
        // 字符串示例函数导入
        // void __cdecl TestStringArgumentsInOut(const wchar_t* inString, wchar_t* outString, int bufferSize);
        [DllImport(DLLManager._dllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private extern static void TestStringArgumentsFixLength(string inString, StringBuilder outString, int bufferSize);

        // NATIVELIB_API void __cdecl TestStringArgumentOut(int id, wchar_t** ppString);
        [DllImport(DLLManager._dllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TestStringArgumentOut")]
        private extern static void TestStringArgumentOutIntPtr(int id, ref IntPtr outString);
        
        [DllImport(DLLManager._dllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private extern static void TestStringArgumentOut(int id, ref string outString);

        // void __cdecl TestStringMarshalArguments(const char* inAnsiString, const wchar_t* inUnicodeString, wchar_t* outUnicodeString, int outBufferSize)
        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static void TestStringMarshalArguments(
            [MarshalAs(UnmanagedType.LPStr)] string inAnsiString, 
            [MarshalAs(UnmanagedType.LPWStr)] string inUnicodeString,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder outStringBuffer,
            int outBufferSize);
        
        // void wchar_t* __cdecl TestStringAsResult()
        [DllImport(DLLManager._dllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "TestStringAsResult")]
        private extern static IntPtr TestStringAsResultIntPtr(int id);

        [DllImport(DLLManager._dllName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        private extern static string TestStringAsResult(int id);

        // BSTR* __cdecl TestBSTRString(BSTR* ppString)
        //[DllImport(DLLManager.DLLName, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        //private extern static IntPtr TestBSTRString(out IntPtr bstrPtr);
        

        static void Test()
        {
            TestMarshalArguments();

            TestStringArgumentsFixLength();

            TestArgumentsOut();

            TestStringResult();

            TestBSTR();

            UnityEngine.Debug.LogFormat("\r\n按任意键退出...");
            Console.Read();
        }



        /// <summary>
        /// 拷贝字符串，托管代码分配内存。
        /// </summary>
        private static void TestStringArgumentsFixLength()
        {
            string inString = "This is a input string.";

            int bufferSize = inString.Length;
            StringBuilder sb = new StringBuilder(bufferSize);

            // 如果初始化了一个容量为N的StringBuilder对象，事实上封送拆收器提供了N+1缓冲区。
            // 这额外多出的一个字符空间是由于非托管字符串需要一个空字符结尾，而StringBuilder则不用。
            TestStringArgumentsFixLength(inString, sb, bufferSize + 1);

            // 如果上面函数调用的最后一个参数用StringBuilder对象的属性指定。虽然运行结果正确，但调试器
            // 将不能单步执行进非托管代码中。这有可能是.NET Framework的小BUG。
            // 这个问题浪费了我们一个下午的时间。
            // TestStringArgumentsFixLength(inString, sb, sb.Capacity + 1);

            UnityEngine.Debug.LogFormat("Original: {0}", inString);
            UnityEngine.Debug.LogFormat("Copied: {0}", sb.ToString());
        }
        
        /// <summary>
        /// 作为参数返回字符串，由非托管函数分配内存。
        /// </summary>
        private static void TestArgumentsOut()
        {
            // 1. IntPtr
            string strResult;
            IntPtr strIntPtr = IntPtr.Zero;
            TestStringArgumentOutIntPtr(1, ref strIntPtr);
            strResult = Marshal.PtrToStringUni(strIntPtr);
            // 对于IntPtr传递的变量，需要手工释放非托管内存
            Marshal.FreeCoTaskMem(strIntPtr);
            UnityEngine.Debug.LogFormat("Return string IntPtr: {0}", strResult);

            // 2. String
            TestStringArgumentOut(2, ref strResult);
            UnityEngine.Debug.LogFormat("Return string value: {0}", strResult);
        }

        private static void TestMarshalArguments()
        {
            string string1 = "Hello";
            string string2 = "世界！";
            int outBufferSize = string1.Length + string2.Length + 2;
            StringBuilder outBuffer = new StringBuilder(outBufferSize);
            TestStringMarshalArguments(string1, string2, outBuffer, outBufferSize);

            UnityEngine.Debug.LogFormat("非托管函数返回的字符串为：{0}", outBuffer.ToString());
        }

        /// <summary>
        /// 作为函数返回值，返回字符串结果
        /// </summary>
        private static void TestStringResult()
        {
            // 1. IntPtr
            string result;
            IntPtr strPtr = TestStringAsResultIntPtr(1);
            result = Marshal.PtrToStringUni(strPtr);
            // 对于IntPtr传递的变量，需要手工释放非托管内存
            Marshal.FreeCoTaskMem(strPtr);
            UnityEngine.Debug.LogFormat("Return string IntPtr: {0}", result);

            // 2. String
            result = TestStringAsResult(2);
            UnityEngine.Debug.LogFormat("Return string value: {0}", result);
        }

        /// <summary>
        /// 用BSTR作为函数结果返回值和参数
        /// </summary>
        private static void TestBSTR()
        {
            IntPtr pString = IntPtr.Zero;
            IntPtr result = IntPtr.Zero;

            //result = TestBSTRString(out pString);

            //if (IntPtr.Zero != pString)
            //{
            //    string argString = Marshal.PtrToStringBSTR(pString);
            //    UnityEngine.Debug.LogFormat("参数传出的BSTR值：{0}", argString);

            //    // 释放BSTR
            //    Marshal.FreeBSTR(pString);
            //}

            //if (IntPtr.Zero != result)
            //{
            //    string retString = Marshal.PtrToStringBSTR(result);
            //    UnityEngine.Debug.LogFormat("函数返回的BSTR值：{0}", retString);

            //    // 释放BSTR
            //    Marshal.FreeBSTR(result);
            //}

        }
    }
}
