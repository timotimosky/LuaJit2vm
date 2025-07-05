using MarshalFullExample;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
[UnityEditor.CustomEditor(typeof(PinvokeTest))]
class TestScriptEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        var ts = target as PinvokeTest;

        if (GUILayout.Button("Call Auto API"))
            ts.CallAutoAPI();
        if (GUILayout.Button("Call PInvoke (requires editor reboot to update NativePlugin"))
            ts.CallPInvokeAPI();
    }
}
#endif


public class PinvokeTest : MonoBehaviour
{

    public void CallAutoAPI()
    {
        Debug.Log("Calling Auto API");

        var val = Struct_Pinvoke.simple_func();
        Debug.Log(string.Format("simple_func: {0}", val));

        var sum = Struct_Pinvoke.sum(2.3f, 1.2f);
        Debug.Log(string.Format("sum: {0}", sum));

        string some_string = "HelloWorld!";
        var len = Struct_Pinvoke.string_length(some_string);
        Debug.Log(string.Format("Length of [{0}] is {1}", some_string, len));

        int[] _intlist = new int[32];
        _intlist[0] = 1;
        var ss = new ANativeStruct(123, 45.67f, _intlist,  "hello");
        var result = Struct_Pinvoke.send_struct(ref ss);
        Debug.Log(string.Format("SendStruct result [{0}]", result));

        ss = Struct_Pinvoke.recv_struct();
        result = Struct_Pinvoke.send_struct(ref ss);
        Debug.Log(string.Format("RecvStruct result [{0}]", result));
    }

    public void CallPInvokeAPI()
    {
        var val = Struct_Pinvoke.simple_func();
        Debug.Log(string.Format("simple_func: {0}", val));

        var sum = Struct_Pinvoke.sum(2.3f, 1.2f);
        Debug.Log(string.Format("sum: {0}", sum));

        string some_string = "HelloWorld!";
        var len = Struct_Pinvoke.string_length(some_string);
        Debug.Log(string.Format("Length of [{0}] is {1}", some_string, len));

        int[] _intlist = new int[32];
        _intlist[0] = 1;
        var ss = new ANativeStruct(123, 45.67f, _intlist, "hello");


        var result = Struct_Pinvoke.send_struct(ref ss);
        Debug.Log(string.Format("SendStruct result [{0}]", result));

        ss = Struct_Pinvoke.recv_struct();
        Debug.Log(string.Format("RecvStruct result [{0}]", ss));
    }

    // Start is called before the first frame update
    void Start()
    {
        CallAutoAPI();
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
