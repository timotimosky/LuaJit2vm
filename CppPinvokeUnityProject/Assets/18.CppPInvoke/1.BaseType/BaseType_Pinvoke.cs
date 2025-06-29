using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System;

//basetype: int、float、string
public class BaseType_Pinvoke 
{

    [DllImport(DLLManager._dllName)]
    private static extern float get_float();

    [DllImport(DLLManager._dllName)]
    private static extern void get_float_ptr(ref float value);

    [DllImport(DLLManager._dllName)]
    private static extern void get_float_ref(ref float value);

    //字符串必须声明编码方式
    [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void get_string_value(ref string value);


    [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void set_string_value(string s);

    [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private static extern void get_string_value(ref StringBuilder ptrStr);



    public static  void TestBaseType()
    {
        // 获取float类型
        float float_value = get_float();
        Debug.Log("get_float_value\t " + float_value);

        // 获取float类型 指针
        float float_ptr = new float();
        get_float_ptr(ref float_ptr);
        Debug.Log("get_float_ptr\t " + float_ptr);

        // 获取float类型 引用
        float float_ref = new float();
        get_float_ref(ref float_ref);
        Debug.Log("get_float_ref\t " + float_ref);

        //string
        string string_ref = "";
        get_string_value(ref string_ref);
        Debug.Log("string_ref\t " + string_ref);

        // 传入字符串
        string s = "hello world";
        set_string_value(s);

        // 获取字符串
        StringBuilder str = new StringBuilder();
        get_string_value(ref str);
        Debug.Log("get_string_value " + str.ToString());

    }
}