using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MarshalFullExample;

namespace MarshalPInvoke
{
   public class ClassWrap
    {
        // void __cdecl GetEmployeeInfo(PMSEMPLOYEE pEmployee)
        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private extern static void GetEmployeeInfo([In, Out] GameObjectCpp employee);

        // void __cdecl TestStructArgument(PSIMPLESTRUCT pStruct);
        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static void TestStructArgumentByRef(ManagedClassBlittable argClass);

        // void __cdecl TestReturnStructFromArg(PSIMPLESTRUCT* pStruct);
        [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static void TestReturnStructFromArg(out ManagedClassBlittable outObject);

        public static void TestAll()
        {
            TestClassNonBlittlable();

            TestClassBlittalbe();

            TestPointer2Pointer();
        }

        private static void TestClassNonBlittlable()
        {
            GameObjectCpp employee = new GameObjectCpp();
            employee.EmployeeID = 10001;

            GetEmployeeInfo(employee);

           UnityEngine.Debug.LogFormat("\n员工信息:");
            UnityEngine.Debug.LogFormat("ID: {0}", employee.EmployeeID);
            UnityEngine.Debug.LogFormat("工龄:{0}", employee.EmployedYear);
            UnityEngine.Debug.LogFormat("Alias: {0}", employee.Alias);
            UnityEngine.Debug.LogFormat("姓名: {0}", employee.DisplayName);
        }

        private static void TestClassBlittalbe()
        {
            ManagedClassBlittable blittableObject = new ManagedClassBlittable();
            blittableObject.IntValue = 1;
            blittableObject.ShortValue = 2;
            blittableObject.FloatValue = 3;
            blittableObject.DoubleValue = 4.5;

            TestStructArgumentByRef(blittableObject);

            UnityEngine.Debug.LogFormat("\n结构体新数据：int = {0}, short = {1}, float = {2:f6}, double = {3:f6}",
                blittableObject.IntValue, blittableObject.ShortValue, blittableObject.FloatValue, blittableObject.DoubleValue);
        }

        private static void TestPointer2Pointer()
        {
            ManagedClassBlittable outObject;

            TestReturnStructFromArg(out outObject);

            UnityEngine.Debug.LogFormat("\n结构体新数据：int = {0}, short = {1}, float = {2:f6}, double = {3:f6}",
                outObject.IntValue, outObject.ShortValue, outObject.FloatValue, outObject.DoubleValue);
        }


        private static void Test()
        {
            NameEntityType allType = NameEntityType.OrganizationName | NameEntityType.PersonName | NameEntityType.PlaceName;
            using (NameFinderWrapper nameFinder = new NameFinderWrapper(allType))
            {
                bool isInit = nameFinder.Initialize(@".\data");
                if (isInit)
                {
                    List<NameEntity> nameResults;
                    string text = @"今天，来自全国各地的优秀学子聚集北京。在微软亚洲研究院听取了洪小文院长、王坚院长和郭百宁院长的精彩演讲。";
                    UnityEngine.Debug.LogFormat("输入文本：{0}{1}", Environment.NewLine, text);
                    bool isSuccess = nameFinder.FindNames(text, out nameResults);
                    if (isSuccess)
                    {
                        UnityEngine.Debug.LogFormat("名字解析成功，结果如下：");
                        UnityEngine.Debug.LogFormat("\t     名字\t\t类型\t     起始位置\t长度\t模型概率");
                        UnityEngine.Debug.LogFormat("          -------------------------------------------------------------");
                        foreach (NameEntity name in nameResults)
                        {
                            UnityEngine.Debug.LogFormat("{0, 15}\t{1, 18}\t{2, -4}\t{3, 2}\t{4, -4}",
                                name.Name,
                                name.Type,
                                name.HighlightBegin,
                                name.HighlightLength,
                                name.Score);
                        }

                    }
                }
            }
        }

    }
}
