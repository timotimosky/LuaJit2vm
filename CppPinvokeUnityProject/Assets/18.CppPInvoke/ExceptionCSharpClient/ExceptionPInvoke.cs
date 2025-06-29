using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Native
{
    // 定义非托管异常的枚举类型
    public enum  UnmanagedExceptionType
    {
        DivedeByZero,
		AccessInvalidMemory,
		ThrowErrorCode,
		ThrowCustomException,
		ThrowStdException
    };


    internal class ExceptionPInvoke
    {

        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "CauseUnmanagedExceptions")]
        private extern static void PInvokeCauseUnmanagedExceptions(UnmanagedExceptionType type);

        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "ThrowUnmanagedExceptions")]
        private extern static void PInvokeThrowUnmanagedExceptions(UnmanagedExceptionType type);

        public static void CauseUnmanagedException()
        {
            try
            {
                PInvokeCauseUnmanagedExceptions(UnmanagedExceptionType.DivedeByZero);
            }
            catch (DivideByZeroException e)
            {
                UnityEngine.Debug.LogFormat("[*]DivideByZeroException: {0}", e.Message);
            }

            try
            {
                PInvokeCauseUnmanagedExceptions(UnmanagedExceptionType.AccessInvalidMemory);
            }
            catch (AccessViolationException e)
            {
                UnityEngine.Debug.LogFormat("[*]AccessViolationException: {0}", e.Message);
            }

        }

        public static void ThrowUnmanagedException()
        {
            try
            {
                PInvokeThrowUnmanagedExceptions(UnmanagedExceptionType.ThrowErrorCode);
            }
            catch (SEHException e)
            {
                UnityEngine.Debug.LogFormat("[*]ThrowErrorCode: {0}", e.Message);
            }

            try
            {
                PInvokeThrowUnmanagedExceptions(UnmanagedExceptionType.ThrowCustomException);
            }
            catch (SEHException e)
            {
                UnityEngine.Debug.LogFormat("[*]ThrowCustomException: {0}", e.Message);
            }

            try
            {
                PInvokeThrowUnmanagedExceptions(UnmanagedExceptionType.ThrowStdException);
            }
            catch (SEHException e)
            {
                UnityEngine.Debug.LogFormat("[*]ThrowStdException: {0}", e.Message);
            }

        }
    }
}
