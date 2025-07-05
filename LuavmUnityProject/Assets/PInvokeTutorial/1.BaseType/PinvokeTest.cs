using MarshalFullExample;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarshalStruct_StructInStruct;
public class PinvokeTest : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        //DLLManager.LoadDLL_Win32();
        BaseType_Pinvoke.TestBaseType();
       // Struct_Pinvoke.Test();
        // Array_Pinvoke.Test();

        //RetErrorCodeDemo.TestFormatErrorMsg();

        //RetErrorCodeDemo.TestErrorMsgByWin32Exception();

        // RetErrorCodeDemo.TestErrorMsgByWin32ExceptionDefault();

       // MarshalStruct_StructInStruct.TestAll();

       // TestCLR();
    }

    private static void TestCLR()
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
