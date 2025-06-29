using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// c#声明的数组
/// </summary>
public class Array_Pinvoke 
{
    [DllImport(DLLManager._dllName)]
    private static extern void get_int_arr1(ref int arr, int count);

    [DllImport(DLLManager._dllName)]
    private static extern int get_int_arr2(int[] arr, int count);

    [DllImport(DLLManager._dllName)]
    private static extern void get_int_arr3(IntPtr arr, int count);


    // 简单数组示例
    // UINT __cdecl TestArrayOfChar(char charArray[], int arraySize);
    [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private extern static uint TestArrayOfChar([In, Out] char[] charArray, int arraySize);

    // int __cdecl TestArrayOfInt(int intArray[], int arraySize);
    [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl)]
    private extern static int TestArrayOfInt(int[] intArray, int arraySize);

    // 字符串数组示例
    // void __cdecl TestArrayOfString(char* ppStrArray[], int size)
    [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
    private extern static void TestArrayOfString([In, Out] string[] charArray, int arraySize);

    // int __cdecl TestRefArrayOfString(void** strArray, int* size)
    [DllImport(DLLManager._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    private extern static int TestRefArrayOfString(out IntPtr charArray, out int arraySize);


    public static void Test()
    {

        TestIntArray();

        //TestCharArray();

        //TestStringArray();

        //TestRefStringArray();

        //int[] intArray = new int[] { 1, 2, 3, 4, 5, 6, 7 };
        //int sum = TestArrayOfInt(intArray, intArray.Length);
        //Debug.Log("\n调用前整数数组中所有元素之和为" + sum);
        //foreach (int i in intArray)
        //{
        //    Debug.Log(i);
        //}
    }
    private static void TestIntArray()
    {
        // 获取int型数组 方法1
        int[] arr1 = new int[10];
        get_int_arr1(ref arr1[0], arr1.Length);
        Debug.Log("get_int_arr1  5 " + arr1[5]);


        // 获取int型数组 方法2
        int[] arr2 = new int[10];
        get_int_arr2(arr2, arr2.Length);
        Debug.Log("\n调用后整数数组元素为：");
        foreach (int i in arr2)
        {
            Debug.Log(i);
        }


        // 获取int型数组 方法3
        int arr3_length = 10;
        int arr3_size = Marshal.SizeOf(typeof(int)) * arr3_length;
        IntPtr arr3_buffer = Marshal.AllocHGlobal(arr3_size);
        get_int_arr3(arr3_buffer, arr3_length);
        int arr3_test_value = (int)Marshal.PtrToStructure(arr3_buffer + 5 * Marshal.SizeOf(typeof(int)), typeof(int));
        Debug.Log("get_int_arr3 " + arr3_test_value);
    }

    private static void TestCharArray()
    {
        char[] charArray = new char[] { 'a', '1', 'b', '2', 'c', '3', '4' };
        UnityEngine.Debug.LogFormat("\n调用前字符数组元素为：");
        foreach (char c in charArray)
        {
            Console.Write("{0} ", c);
        }

        uint digitCount = TestArrayOfChar(charArray, charArray.Length);

        UnityEngine.Debug.LogFormat("\n调用前字符数组中数字个数为：{0}", digitCount);

        UnityEngine.Debug.LogFormat("\n调用后字符数组元素为：");
        foreach (char c in charArray)
        {
            Console.Write("{0} ", c);
        }
    }

    private static void TestStringArray()
    {
        string[] strings = new string[] {
                "This is the first string.",
                "Those are brown horse.",
                "The quick brown fox jumps over a lazy dog." };

        UnityEngine.Debug.LogFormat("\n原始字符串数组中的元素为：");
        foreach (string originalString in strings)
        {
            UnityEngine.Debug.LogFormat(originalString);
        }

        TestArrayOfString(strings, strings.Length);

        UnityEngine.Debug.LogFormat("修改后字符串数组中的元素为：");
        foreach (string reversedString in strings)
        {
            UnityEngine.Debug.LogFormat(reversedString);
        }
    }

    private static void TestRefStringArray()
    {
        IntPtr arrayPtr;

        // 因为数组是在非托管代码内分配的，所以需要通过返回值或参数给出
        // 在这里arraySize和returnCount的返回值应该是一样的
        int arraySize;
        int returnCount = TestRefArrayOfString(out arrayPtr, out arraySize);
        // 根据返回值确定字符串数量，在托管代码中声明相对应的指针数组
        IntPtr[] arrayPtrs = new IntPtr[returnCount];
        // 将非托管数组中的内容拷贝到托管代码中
        Marshal.Copy(arrayPtr, arrayPtrs, 0, returnCount);

        UnityEngine.Debug.LogFormat("\n返回的字符串数组中元素的个数为：{0}", returnCount);
        UnityEngine.Debug.LogFormat("字符串元素：");
        // 声明字符串数组，用于存放最终的结果
        string[] strings = new string[returnCount];
        for (int i = 0; i < returnCount; i++)
        {
            strings[i] = Marshal.PtrToStringUni(arrayPtrs[i]);
            // 释放非托管字符串内存
            Marshal.FreeCoTaskMem(arrayPtrs[i]);
            UnityEngine.Debug.LogFormat("#{0}: {1}", i, strings[i]);
        }

        // 释放非托管字符串数组内存
        Marshal.FreeCoTaskMem(arrayPtr);
    }
}
