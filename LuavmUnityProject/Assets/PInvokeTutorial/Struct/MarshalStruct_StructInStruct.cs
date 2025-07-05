using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalStruct_StructInStruct
{
    class MarshalStruct_StructInStruct
    {
        //typedef struct _PERSONNAME
        //{
        //    char* first;
        //    char* last;
        //    char* displayName;
        //} PERSONNAME, *PPERSONNAME;
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct PersonName
        {
            public string first;
            public string last;
            public string displayName;
        }

        //typedef struct _PERSON
        //{
        //    PPERSONNAME pName;
        //    int age; 
        //} PERSON, *PPERSON;
        [StructLayout(LayoutKind.Sequential)]
        public struct Person
        {
            public IntPtr name;
            public int age;
        }

        //void __cdecl TestStructInStructByRef(PPERSON pPerson);
        [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void TestStructInStructByRef(ref Person person);

        //typedef struct _PERSON2
        //{
        //    PERSONNAME name;
        //    int age; 
        //} PERSON2, *PPERSON2;
        [StructLayout(LayoutKind.Sequential)]
        public struct Person2
        {
            public PersonName name;
            public int age;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Person2_Flattened
        {
            public string first;
            public string last;
            public string displayName;
            public int age;
        }

        //void __cdecl TestStructInStructByVal(PPERSON2 pPerson);
        [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void TestStructInStructByVal(ref Person2 person);

        [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void TestStructInStructByVal(ref Person2_Flattened person);
        
        public static void TestAll()
        {
            TestStructAsRefField();

            TestStructAsValField();

            TestStructAsValFieldFlattened();

            UnityEngine.Debug.LogFormat("\r\n按任意键退出...");
            Console.Read();
        }

        private static void TestStructAsRefField()
        {
            UnityEngine.Debug.LogFormat("\n结构体作为引用类型成员");
            // 创建名字
            PersonName name = new PersonName();
            name.last = "Cui";
            name.first = "Xiaoyuan";

            // 创建人
            Person person = new Person();
            person.age = 27;

            IntPtr nameBuffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(name));
            Marshal.StructureToPtr(name, nameBuffer, false);

            person.name = nameBuffer;

            UnityEngine.Debug.LogFormat("调用前显示姓名为：{0}", name.displayName);
            TestStructInStructByRef(ref person);

            PersonName newValue =
                (PersonName)Marshal.PtrToStructure(person.name, typeof(PersonName));

            // 释放在非托管代码中分配的PersonName实例内存
            Marshal.DestroyStructure(nameBuffer, typeof(PersonName));

            UnityEngine.Debug.LogFormat("调用后显示姓名为：{0}", newValue.displayName);

        }

        private static void TestStructAsValField()
        {
            UnityEngine.Debug.LogFormat("\n结构体作为值类型成员");
            Person2 person = new Person2();
            person.name.last = "Huang";
            person.name.first = "Jizhou";
            person.name.displayName = string.Empty;
            person.age = 26;
            
            TestStructInStructByVal(ref person);
        }

        private static void TestStructAsValFieldFlattened()
        {
            UnityEngine.Debug.LogFormat("\n结构体作为值类型成员（flattened）");
            Person2_Flattened person = new Person2_Flattened();
            person.last = "Huang";
            person.first = "Jizhou";
            person.displayName = string.Empty;
            person.age = 26;

            TestStructInStructByVal(ref person);
        }

    }
}
