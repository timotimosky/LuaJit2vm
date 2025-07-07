using System;
using System.Runtime.InteropServices;
using Python.Runtime.Native;

namespace Python.Runtime
{
    public unsafe partial class Runtime
    {
        // BonusGamesHack: Unity使用的C#版本兼容的库调用方式
        internal static class Delegates
        {
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void Py_IncRef(BorrowedReference b);

            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void Py_DecRef(StolenReference s);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void Py_Initialize();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void Py_InitializeEx(int i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int Py_IsInitialized();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void Py_Finalize();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern PyThreadState* Py_NewInterpreter();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void Py_EndInterpreter(PyThreadState* state);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern PyThreadState* PyThreadState_New(PyInterpreterState* state);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern PyThreadState* PyThreadState_Get();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern PyThreadState* _PyThreadState_UncheckedGet();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyGILState_Check();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern PyGILState PyGILState_Ensure();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyGILState_Release(PyGILState state);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern PyThreadState* PyGILState_GetThisThreadState();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int Py_Main(int argc, IntPtr argv);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyEval_InitThreads();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyEval_ThreadsInitialized();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyEval_AcquireLock();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyEval_ReleaseLock();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyEval_AcquireThread(PyThreadState* state);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyEval_ReleaseThread(PyThreadState* state);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern PyThreadState* PyEval_SaveThread();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyEval_RestoreThread(PyThreadState* state);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyEval_GetBuiltins();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyEval_GetGlobals();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyEval_GetLocals();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr Py_GetProgramName();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void Py_SetProgramName(IntPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr Py_GetPythonHome();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void Py_SetPythonHome(IntPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr Py_GetPath();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void Py_SetPath(IntPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr Py_GetVersion();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr Py_GetPlatform();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr Py_GetCopyright();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr Py_GetCompiler();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr Py_GetBuildInfo();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyRun_SimpleStringFlags(StrPtr ptr, in PyCompilerFlags flags);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyRun_StringFlags(StrPtr s_ptr, RunFlagType t, BorrowedReference b_ref, BorrowedReference b_ref_1, in PyCompilerFlags flags);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyEval_EvalCode(BorrowedReference b_ref, BorrowedReference b_ref_1, BorrowedReference b_ref_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference Py_CompileStringObject(StrPtr ptr, BorrowedReference b_ref, int i, in PyCompilerFlags flags, int i_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyImport_ExecCodeModule(StrPtr ptr, BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_HasAttrString(BorrowedReference b_ref, StrPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_GetAttrString(BorrowedReference b_ref, StrPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_SetAttrString(BorrowedReference b_ref, StrPtr ptr,BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_HasAttr(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_GetAttr(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_SetAttr(BorrowedReference b_ref, BorrowedReference b_ref_1, BorrowedReference b_ref_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_GetItem(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_SetItem(BorrowedReference b_ref, BorrowedReference b_ref_1, BorrowedReference b_ref_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_DelItem(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_GetIter(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_Call(BorrowedReference b_ref, BorrowedReference b_ref_1, BorrowedReference b_ref_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_CallObject(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_RichCompareBool(BorrowedReference b_ref, BorrowedReference b_ref_1, int i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_IsInstance(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_IsSubclass(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyObject_ClearWeakRefs(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyCallable_Check(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_IsTrue(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_Not(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_Size(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_Hash(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_Repr(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_Str(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_Type(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_Dir(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_GetBuffer(BorrowedReference b_ref, out Py_buffer buffer, int i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyBuffer_Release(ref Py_buffer buffer);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyBuffer_SizeFromFormat(StrPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyBuffer_IsContiguous(ref Py_buffer buffer, char c);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr PyBuffer_GetPointer(ref Py_buffer buffer, nint[] iv);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyBuffer_FromContiguous(ref Py_buffer buffer, IntPtr ptr, IntPtr ptr_1, char c);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyBuffer_ToContiguous(IntPtr ptr, ref Py_buffer buffer, IntPtr ptr_1, char c);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyBuffer_FillContiguousStrides(int i, IntPtr ptr, IntPtr ptr_1, int i_1, char c);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyBuffer_FillInfo(ref Py_buffer buffer, BorrowedReference b_ref, IntPtr ptr, IntPtr ptr_1, int i, int i_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Long(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Float(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern bool PyNumber_Check(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyLong_FromLongLong(long l);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyLong_FromUnsignedLongLong(ulong u);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyLong_FromString(StrPtr s_ptr, IntPtr ptr, int i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern long PyLong_AsLongLong(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern ulong PyLong_AsUnsignedLongLong(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyLong_FromVoidPtr(IntPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr PyLong_AsVoidPtr(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyFloat_FromDouble(double d);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyFloat_FromString(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern double PyFloat_AsDouble(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Add(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Subtract(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Multiply(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_TrueDivide(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_And(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Xor(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Or(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Lshift(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Rshift(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Power(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Remainder(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlaceAdd(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlaceSubtract(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlaceMultiply(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlaceTrueDivide(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlaceAnd(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlaceXor(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlaceOr(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlaceLshift(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlaceRshift(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlacePower(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_InPlaceRemainder(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Negative(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Positive(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyNumber_Invert(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern bool PySequence_Check(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PySequence_GetItem(BorrowedReference b_ref, nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PySequence_SetItem(BorrowedReference b_ref, nint i, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PySequence_DelItem(BorrowedReference b_ref, nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PySequence_GetSlice(BorrowedReference b_ref, nint i, nint i_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PySequence_SetSlice(BorrowedReference b_ref, nint i, nint i_1, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PySequence_DelSlice(BorrowedReference b_ref, nint i, nint i_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern nint PySequence_Size(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PySequence_Contains(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PySequence_Concat(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PySequence_Repeat(BorrowedReference b_ref, nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PySequence_Index(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PySequence_Count(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PySequence_Tuple(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PySequence_List(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr PyBytes_AsString(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyBytes_FromString(IntPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyByteArray_FromStringAndSize(IntPtr ptr, nint i);
            
            // BonusGamesHack: ByteArray获取头指针的C接口
            // 详见C-API: https://docs.python.org/3/c-api/bytearray.html
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr PyByteArray_AsString(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyBytes_Size(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr PyUnicode_AsUTF8(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyUnicode_DecodeUTF16(IntPtr ptr, nint i, IntPtr ptr_1, IntPtr ptr_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyUnicode_GetLength(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyUnicode_ReadChar(BorrowedReference b_ref, nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyUnicode_AsUTF16String(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyUnicode_FromOrdinal(int i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyUnicode_InternFromString(StrPtr s_ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyUnicode_Compare(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyDict_New();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyDict_GetItem(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyDict_GetItemString(BorrowedReference b_ref, StrPtr s_ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyDict_SetItem(BorrowedReference b_ref, BorrowedReference b_ref_1, BorrowedReference b_ref_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyDict_SetItemString(BorrowedReference b_ref, StrPtr ptr,BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyDict_DelItem(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyDict_DelItemString(BorrowedReference b_ref, StrPtr s_ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyMapping_HasKey(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyDict_Keys(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyDict_Values(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyDict_Items(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyDict_Copy(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyDict_Update(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyDict_Clear(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyDict_Size(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PySet_New(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PySet_Add(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PySet_Contains(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyList_New(nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyList_GetItem(BorrowedReference b_ref, nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyList_SetItem(BorrowedReference b_ref, nint i, StolenReference s_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyList_Insert(BorrowedReference b_ref, nint i, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyList_Append(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyList_Reverse(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyList_Sort(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyList_GetSlice(BorrowedReference b_ref, nint i, nint i_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyList_SetSlice(BorrowedReference b_ref, nint i, nint i_1, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern nint PyList_Size(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyTuple_New(nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyTuple_GetItem(BorrowedReference b_ref, nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyTuple_SetItem(BorrowedReference b_ref, nint i, StolenReference s_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyTuple_GetSlice(BorrowedReference b_ref, nint i, nint i_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyTuple_Size(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyIter_Check(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyIter_Next(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyModule_New(StrPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyModule_GetDict(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyModule_AddObject(BorrowedReference b_ref, StrPtr s_ptr, IntPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyImport_Import(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyImport_ImportModule(StrPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyImport_ReloadModule(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyImport_AddModule(StrPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyImport_GetModuleDict();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PySys_SetArgvEx(int i, IntPtr ptr, int i_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PySys_GetObject(StrPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PySys_SetObject(StrPtr ptr,BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyType_Modified(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern bool PyType_IsSubtype(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyType_GenericNew(BorrowedReference b_ref, BorrowedReference b_ref_1, BorrowedReference b_ref_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyType_GenericAlloc(BorrowedReference b_ref, nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyType_Ready(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference _PyType_Lookup(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_GenericGetAttr(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_GenericSetAttr(BorrowedReference b_ref, BorrowedReference b_ref_1, BorrowedReference b_ref_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyObject_GC_Del(StolenReference s_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyObject_GC_IsTracked(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyObject_GC_Track(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyObject_GC_UnTrack(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void _PyObject_Dump(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr PyMem_Malloc(nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr PyMem_Realloc(IntPtr ptr, nint i);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyMem_Free(IntPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyErr_SetString(BorrowedReference b_ref, StrPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyErr_SetObject(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyErr_ExceptionMatches(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyErr_GivenExceptionMatches(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyErr_NormalizeException(ref NewReference n_ref, ref NewReference n_ref_1, ref NewReference n_ref_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyErr_Occurred();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyErr_Fetch(out NewReference n_ref, out NewReference n_ref_1, out NewReference n_ref_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyErr_Restore(StolenReference s_ref, StolenReference s_ref_1, StolenReference s_ref_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyErr_Clear();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyErr_Print();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyCell_Get(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyCell_Set(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyGC_Collect();
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyCapsule_New(IntPtr ptr, IntPtr ptr_1, IntPtr ptr_2);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr PyCapsule_GetPointer(BorrowedReference b_ref, IntPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyCapsule_SetPointer(BorrowedReference b_ref, IntPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, EntryPoint = "PyLong_AsSize_t", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern uint PyLong_AsUnsignedSize_t(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, EntryPoint = "PyLong_AsSsize_t", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyLong_AsSignedSize_t(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern BorrowedReference PyDict_GetItemWithError(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyException_GetCause(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyException_GetTraceback(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void PyException_SetCause(BorrowedReference b_ref, StolenReference s_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyException_SetTraceback(BorrowedReference b_ref, BorrowedReference b_ref_1);
            
            [DllImport(Runtime.PYTHON_DLL, EntryPoint = "PyThreadState_SetAsyncExc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyThreadState_SetAsyncExcLLP64(uint u, BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, EntryPoint = "PyThreadState_SetAsyncExc", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int PyThreadState_SetAsyncExcLP64(ulong u, BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyObject_GenericGetDict(BorrowedReference b_ref, IntPtr ptr);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern IntPtr PyType_GetSlot(BorrowedReference b_ref, TypeSlotID id);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern NewReference PyType_FromSpecWithBases(in NativeTypeSpec spec, BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern void _Py_NewReference(BorrowedReference b_ref);
            
            [DllImport(Runtime.PYTHON_DLL, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int _Py_IsFinalizing();

            [DllImport(Runtime.PYTHON_DLL, EntryPoint = "Py_NoSiteFlag", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true, CharSet = CharSet.Ansi)]
            internal static extern int* Py_NoSiteFlag_Function();

            internal static int* Py_NoSiteFlag { get; }
        }
    }
}
