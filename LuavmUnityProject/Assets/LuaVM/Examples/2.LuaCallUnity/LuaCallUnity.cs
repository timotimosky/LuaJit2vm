using AOT;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class LuaCallUnity : MonoBehaviour
{

    delegate void UnityFunc();
    [AOT.MonoPInvokeCallback(typeof(UnityFunc))]
    static void OnDebug(string log)
    {
        Debug.Log(log);
    }


    [DllImport("__Internal")]
    static extern void RegisterCallback(UnityFunc func);
    //接受回调的 C 代码如下所示
    // 注意： 确保从原生方法返回的字符串值是 UTF-8 编码的，并在堆上分配。
    //typedef void (* UnityFunc) ();
    //void RegisterCallback(UnityFunc func) { }

}
