//using System;
//using System.Collections.Generic;
//using System.Text;
//using CppInteropWrapper;

//namespace ExceptionCSharpClient
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            UnityEngine.Debug.LogFormat("----- P/Invoke, Runtime Exception -----");
//            ExceptionPInvoke.CauseUnmanagedException();

//            UnityEngine.Debug.LogFormat("\n----- P/Invoke, Thrown Exception -----");
//            ExceptionPInvoke.ThrowUnmanagedException();

//            UnityEngine.Debug.LogFormat("\n----- Cpp Interop, Thrown Exception -----");
//            TestMCppWrapperException();

//            UnityEngine.Debug.LogFormat("\r\n按任意键退出...");
//            Console.Read();	
//        }

//        private static void TestMCppWrapperException()
//        {
//            try
//            {
//                ThrowUnmanagedExceptionWrapperClass.CallUnmanagedException(UnmanagedExceptionType.ThrowErrorCode);
//            }
//            catch (WrapperException ex)
//            {
//                UnityEngine.Debug.LogFormat("[*]WrapperException: Message-{0}, ErrorCode-{1}", ex.Message, ex.ErrorCode);
//            }

//            try
//            {
//                ThrowUnmanagedExceptionWrapperClass.CallUnmanagedException(UnmanagedExceptionType.ThrowCustomException);
//            }
//            catch (WrapperException ex)
//            {
//                UnityEngine.Debug.LogFormat("[*]WrapperException: Message-{0}, ErrorCode-{1}", ex.Message, ex.ErrorCode);
//            }

//            try
//            {
//                ThrowUnmanagedExceptionWrapperClass.CallUnmanagedException(UnmanagedExceptionType.ThrowStdException);
//            }
//            catch (WrapperException ex)
//            {
//                UnityEngine.Debug.LogFormat("[*]WrapperException: Message-{0}, ErrorCode-{1}", ex.Message, ex.ErrorCode);
//            }
//        }
//    }
//}
