using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalStruct_FieldMarshal
{
    class MarshalStruct_FieldMarshal
    {
        private const string _dllName = "NativeLib.dll";

        //typedef struct _MSEMPLOYEE_EX
        //{
        //    UINT employeeID;
        //    wchar_t* displayName; 
        //    char* alias; 
        //    bool isInRedmond;
        //    BOOL isFamale;
        //} MSEMPLOYEE_EX, *PMSEMPLOYEE_EX;

        [StructLayout(LayoutKind.Sequential)]
        private struct MsEmployeeEx
        {
            public uint EmployeeID;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string DisplayName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string Alias;
            [MarshalAs(UnmanagedType.I1)]
            public bool IsInRedmond;
            public short EmployedYear;
            [MarshalAs(UnmanagedType.Bool)]
            public bool IsFamale;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MsEmployeeEx_Wrong
        {
            public uint EmployeeID;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string DisplayName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string Alias;
            // 如果忘记了指定封送的方式，.NET中bool类型会被默认封送成4字节的Win32 BOOL类型
            // 这将导致结构体在非托管代码中的大小发生改变。
            // [MarshalAs(UnmanagedType.I1)]
            public bool IsInRedmond;
            public short EmployedYear;
            [MarshalAs(UnmanagedType.Bool)]
            public bool IsFamale;
        }

        // void __cdecl GetEmployeeInfoEx(PMSEMPLOYEE_EX pEmployee);
        [DllImport(_dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static void GetEmployeeInfoEx(ref MsEmployeeEx employee);

        // void __stdcall PrintEmployeeInfoEx(MSEMPLOYEE_EX pEmployee);
        [DllImport(_dllName, CallingConvention = CallingConvention.StdCall)]
        private extern static void PrintEmployeeInfoEx(MsEmployeeEx_Wrong employee);

        // 这样显示指定函数入口名，即使不会抛出EntryPointNotFoundException异常，
        // 也不会得到正确的结果，因为内存的布局已经被破坏
        //[DllImport(_dllName, CallingConvention = CallingConvention.StdCall, EntryPoint = "_PrintEmployeeInfoEx@20")]
        //private extern static void PrintEmployeeInfoEx(MsEmployeeEx_Wrong employee);

        static void Main(string[] args)
        {
            TestGetEmployeeEx();

            // 下面这个方法会导致“EntryPointNotFoundException”异常抛出。
            // 由于没有正确的指定结构体内所有字段的封送方式，导致结构体
            // 在封送时的大小与非托管代码不一致。
            // 又由于非托管代码使用stdcall, 使得最终的函数导出名要依赖于参数大小。
            // 所以错误的结构体定义，会使DllImportAttribute类找不到对应的函数入口。
            // TestPrintEmployeeInfoEx();

            UnityEngine.Debug.LogFormat("\r\n按任意键退出...");
            Console.Read();
        }

        private static void TestGetEmployeeEx()
        {
            ShowMarshalSize(typeof(MsEmployeeEx));
            MsEmployeeEx employee = new MsEmployeeEx();
            employee.EmployeeID = 10001;
            GetEmployeeInfoEx(ref employee);

            UnityEngine.Debug.LogFormat("员工信息:");
            UnityEngine.Debug.LogFormat("ID: {0}", employee.EmployeeID);
            UnityEngine.Debug.LogFormat("Alias: {0}", employee.Alias);
            UnityEngine.Debug.LogFormat("姓名: {0}", employee.DisplayName);
            UnityEngine.Debug.LogFormat("性别: {0}", employee.IsFamale ? "女" : "男");
            UnityEngine.Debug.LogFormat("工龄: {0}", employee.EmployedYear);
            UnityEngine.Debug.LogFormat("是否在总部: {0}", employee.IsInRedmond ? "是" : "否");
        }

        private static void TestPrintEmployeeInfoEx()
        {
            ShowMarshalSize(typeof(MsEmployeeEx_Wrong));
            MsEmployeeEx_Wrong employee = new MsEmployeeEx_Wrong();
            employee.EmployeeID = 10001;
            employee.Alias = "xcui";
            employee.DisplayName = "Xiaoyuan Cui";
            employee.IsFamale = false;
            employee.IsInRedmond = false;
            employee.EmployedYear = 2;

            PrintEmployeeInfoEx(employee);
        }

        private static void ShowMarshalSize(Type type)
        {
            UnityEngine.Debug.LogFormat("托管代码定义的结构体({0})在非托管代码中的大小为({1})字节", type.Name, Marshal.SizeOf(type));
        }
    }
}
