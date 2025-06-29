using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalStruct_ExactLayout
{
    class MarshalStruct_ExactLayout
    {
        //#pragma pack(1)
        //typedef struct _MSEMPLOYEE_EX2
        //{
        //    UINT      EmployeeID;       //4 bytes -> Offset 0
        //    USHORT    EmployedYear;     //2 bytes -> Offset 4
        //    BYTE      CurrentLevel;     //1 bytes -> Offset 6
        //    wchar_t*  Alias;            //4 bytes -> Offset 7
        //    wchar_t*  DisplayName;      //4 bytes -> Offset 11
        //    wchar_t*  OfficeAddress;    //4 bytes -> Offset 15
        //    wchar_t*  OfficePhone;      //4 bytes -> Offset 19
        //    wchar_t*  Title;            //4 bytes -> Offset 23
        //    USHORT    RegionId;         //2 bytes -> Offset 27
        //    int       ZipCode;          //4 bytes -> Offset 29
        //    double    CurrentSalary;    //8 bytes -> Offset 33
        //} MSEMPLOYEE_EX2, *PMSEMPLOYEE_EX2;
        //#pragma pack()

        // 由于我们显示指定了最后一个字段，所以不用指定StructLayout的Size属性大小为41
        // 有意思的是Pack属性并不一定要显示指定为1, 保持与非托管代码定义一致。
        // 这时虽然Marshaler显示的大小为48，但是并不会影响到结果。
         [StructLayout(LayoutKind.Explicit)]
        //[StructLayout(LayoutKind.Explicit, Pack = 1)]
        private struct MsEmployeeEx2
        {
            [FieldOffset(0)]    public uint EmployeeID;
            [FieldOffset(4)]    public ushort EmployedYear;
            [FieldOffset(6)]    public byte CurrentLevel;
            [FieldOffset(27)]   public ushort RegionId;
            [FieldOffset(29)]   public int ZipCode;
            [FieldOffset(33)]   public double CurrentSalary;
        }

        [StructLayout(LayoutKind.Explicit, Size = 41)]
        private struct MsEmployeeEx2_Partial
        {
            [FieldOffset(0)]    public uint EmployeeID;
            [FieldOffset(4)]    public ushort EmployedYear;
            [FieldOffset(6)]    public byte CurrentLevel;

            // 这里删除对最后一个字段的定义，所以需要使用StructLayout的Size属性指定大小为41
            //[FieldOffset(27)]   public ushort RegionId;
            //[FieldOffset(29)]   public int ZipCode;
            //[FieldOffset(33)]   public double CurrentSalary;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct MsEmployeeEx2_Partial2
        {
            [FieldOffset(0)]    public uint EmployeeID;
            [FieldOffset(4)]    public ushort EmployedYear;
            [FieldOffset(6)]    public byte CurrentLevel;
            //[FieldOffset(27)]   public ushort RegionId;
            //[FieldOffset(29)]   public int ZipCode;
            //[FieldOffset(33)]   public double CurrentSalary;
        }

        // void __cdecl GetEmployeeInfoEx2(PMSEMPLOYEE_EX2 pEmployee);
        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static void GetEmployeeInfoEx2(ref MsEmployeeEx2 employee);

        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetEmployeeInfoEx2")]
        private extern static void GetEmployeeInfoEx2_PartialStruct(ref MsEmployeeEx2_Partial employee);

        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, EntryPoint = "GetEmployeeInfoEx2")]
        private extern static void GetEmployeeInfoEx2_PartialStruct2(ref MsEmployeeEx2_Partial2 employee);

        //typedef union _SIMPLEUNION
        //{
        //    int i;
        //    double d;
        //} SIMPLEUNION, *PSIMPLEUNION;
        [StructLayout(LayoutKind.Explicit)]
        public struct SimpleUnion
        {
            [FieldOffset(0)]
            public int i;
            [FieldOffset(0)]
            public double d;
        }

        // void __cdecl TestUnion(SIMPLEUNION u, int type)
        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TestUnion(SimpleUnion u, int type);

        //typedef union _SIMPLEUNION2
        //{
        //    int i;
        //    char str[128];
        //} SIMPLEUNION2, *PSIMPLEUNION2;
        [StructLayout(LayoutKind.Explicit, Size = 128)]
        public struct SimpleUnion2_1
        {
            [FieldOffset(0)]
            public int i;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct SimpleUnion2_2
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string str;
        }

        // void __cdecl TestUnion2(SIMPLEUNION2 u, int type)
        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TestUnion2(SimpleUnion2_1 u, int type);

        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void TestUnion2(SimpleUnion2_2 u, int type);

        static void Main(string[] args)
        {
            TestStructExactLayout();

            TestStructExactLayout2();

            // 这个方法有可能导致程序崩溃
            //TestStructExactLayout3();

            TestUnion();

            TestUnion2();

            UnityEngine.Debug.LogFormat("\r\n按任意键退出...");
            Console.Read();
        }

        private static void TestStructExactLayout()
        {
            ShowMarshalSize(typeof(MsEmployeeEx2));

            // 创建一个对象，并赋以初始值
            MsEmployeeEx2 employee = new MsEmployeeEx2();
            employee.EmployeeID = 10001;
            employee.EmployedYear = 1;
            employee.CurrentLevel = 59;
            employee.RegionId = 1000;
            employee.ZipCode = 16;
            employee.CurrentSalary = 123456;

            GetEmployeeInfoEx2(ref employee);

            UnityEngine.Debug.LogFormat("员工信息:");
            UnityEngine.Debug.LogFormat("ID: {0}", employee.EmployeeID);
            UnityEngine.Debug.LogFormat("工龄: {0}", employee.EmployedYear);
            UnityEngine.Debug.LogFormat("职级: {0}", employee.CurrentLevel);
            UnityEngine.Debug.LogFormat("区域代码: {0}", employee.RegionId);
            UnityEngine.Debug.LogFormat("邮政编码: {0}", employee.ZipCode);
            UnityEngine.Debug.LogFormat("工资: {0}", employee.CurrentSalary);
        }

        private static void TestStructExactLayout2()
        {
            ShowMarshalSize(typeof(MsEmployeeEx2_Partial));

            // 创建一个对象，并赋以初始值
            MsEmployeeEx2_Partial employee = new MsEmployeeEx2_Partial();
            employee.EmployeeID = 10001;
            employee.EmployedYear = 1;
            employee.CurrentLevel = 59;

            GetEmployeeInfoEx2_PartialStruct(ref employee);

            UnityEngine.Debug.LogFormat("员工信息:");
            UnityEngine.Debug.LogFormat("ID: {0}", employee.EmployeeID);
            UnityEngine.Debug.LogFormat("工龄: {0}", employee.EmployedYear);
            UnityEngine.Debug.LogFormat("职级: {0}", employee.CurrentLevel);
        }

        private static void TestStructExactLayout3()
        {
            ShowMarshalSize(typeof(MsEmployeeEx2_Partial2));

            // 创建一个对象，并赋以初始值
            MsEmployeeEx2_Partial2 employee = new MsEmployeeEx2_Partial2();
            employee.EmployeeID = 10001;
            employee.EmployedYear = 1;
            employee.CurrentLevel = 59;

            // 由于结构体声明有误，导致封送拆收器错误的将结构体映射到内存中。
            // 这会导致非托管代码读取信息失败，甚至会是应用程序崩溃。
            GetEmployeeInfoEx2_PartialStruct2(ref employee);

            UnityEngine.Debug.LogFormat("员工信息:");
            UnityEngine.Debug.LogFormat("ID: {0}", employee.EmployeeID);
            UnityEngine.Debug.LogFormat("工龄: {0}", employee.EmployedYear);
            UnityEngine.Debug.LogFormat("职级: {0}", employee.CurrentLevel);
        }

        private static void TestUnion()
        {
            SimpleUnion u = new SimpleUnion();
            u.i = 10;
            TestUnion(u, 1);

            u.d = 10.10;
            TestUnion(u, 2);
        }

        private static void TestUnion2()
        {
            SimpleUnion2_1 u1Integer = new SimpleUnion2_1();
            u1Integer.i = 10;
            TestUnion2(u1Integer, 1);

            SimpleUnion2_2 u1String = new SimpleUnion2_2();
            u1String.str = "*** This is a string. ***";
            TestUnion2(u1String, 2);		
        }

        private static void ShowMarshalSize(Type type)
        {
            UnityEngine.Debug.LogFormat("\n托管代码定义的结构体({0})在非托管代码中的大小为({1})字节", type.Name, Marshal.SizeOf(type));
        }

    }
}
