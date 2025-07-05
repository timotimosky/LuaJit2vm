using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;


//3.C#和C++的相互交互，是基于.NET和P/Invoke，那么我们可以同理推导c#和lua的操作，其实质就是对handle进行包装
//传递函数指针

//前面介绍的3种情况都是单向的，即C#向C++传递数据，有的时候也需要C++主动调用C#的函数，
//C#没有明确的函数指针，所以用回调函数，也就是委托包装
//这些函数指针必须是静态，避免被回收。以及是被分配在堆上的

public class PInvokeDelegate : MonoBehaviour
{
#if UNITY_EDITOR
    // pointer handle to the C++ DLL
    public IntPtr libarayHandle;

    InitDelegate Init;
    public delegate void InitDelegate(IntPtr gameObjectNew, IntPtr gameObjectGetTransform, IntPtr transformSetPosition);

    MonoBehaviourUpdateDelegate MonoBehaviourUpdate;
    public delegate void MonoBehaviourUpdateDelegate();
#else

    [DllImport(DLLManager._dllName)]
    static extern void MonoBehaviourUpdate();
     [DllImport(DLLManager._dllName)]
    static extern void Init(IntPtr gameObjectNew,IntPtr gameObjectGetTransform,IntPtr transformSetPosition);
#endif
    //如果在独立的DotNet程序里，不支持泛型跟C++通信，所以要定义委托
    delegate int GameObjectNewDelegate();
    delegate int GameObjectGetTransformDelegate(int thisHandle);
    delegate void TransformSetPositionDelegate(int thisHandle, Vector3 position);

    void Awake()
    {
#if UNITY_EDITOR
        //open the native library
        libarayHandle = DllLoader.LoadDll(DllLoader._dllName);
        Init = DllLoader.GetDelegate<InitDelegate>(libarayHandle, "Init");
        MonoBehaviourUpdate = DllLoader.GetDelegate<MonoBehaviourUpdateDelegate>(libarayHandle, "MonoBehaviourUpdate");
#endif

        //利用Marshal.GetFunctionPointerForDelegate来获取函数的指针，然后传递到c++中进行操作。
#if UNITY_EDITOR
        //init the C++ Library
        ObjectStore.InitObjects(1024);
        //如果在独立的DotNet程序里，不支持泛型跟C++通信，所以要定义委托
        Init(
        Marshal.GetFunctionPointerForDelegate(new GameObjectNewDelegate(GameObjectNew)),
        Marshal.GetFunctionPointerForDelegate(new GameObjectGetTransformDelegate(GameObjectGetTransform)),
        Marshal.GetFunctionPointerForDelegate(new TransformSetPositionDelegate(TransformSetPosition))
       );
#else
        ////将GameObjectNew、GameObjectGetTransform、TransformSetPosition三个函数的指针，都传入C++
        //Init(
        //    Marshal.GetFunctionPointerForDelegate(new Func<int>(GameObjectNew)),
        //    Marshal.GetFunctionPointerForDelegate(new Func<int, int>(GameObjectGetTransform)),
        //    Marshal.GetFunctionPointerForDelegate(new Action<int, Vector3>(TransformSetPosition))
        //    );
#endif
    }

    void Update()
    {
         MonoBehaviourUpdate();
    }

    void OnApplicationQuit()
    {
#if UNITY_EDITOR
        DllLoader.FreeLib(libarayHandle);
        libarayHandle = IntPtr.Zero;
#endif
    }




    #region C# functions for C++ to call
    ////////////////////////////////////////////////////////////////
    static int GameObjectNew()
    {
        Debug.LogError("GameObjectNew---被CPP调用");
        GameObject go = new GameObject();
        return ObjectStore.Store(go);
    }

    static int GameObjectGetTransform(int thisHandle)
    {
        Debug.LogError("GameObjectGetTransform---被CPP调用");
        GameObject go = (GameObject)ObjectStore.Get(thisHandle);
        Transform transform = go.transform;
        return ObjectStore.Store(transform);
    }

    static void TransformSetPosition(int handle, Vector3 position)
    {
        Debug.LogError("TransformSetPosition---被CPP调用");
        Transform t = (Transform)ObjectStore.Get(handle);
        t.position = position;
    }
    #endregion
}
