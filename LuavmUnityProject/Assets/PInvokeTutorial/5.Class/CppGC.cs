using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

//如何编写safe GC P/Invoke code

//用户代码和运行时垃圾收集器(GC)之间的竞争条件。
//. net使用的GC并不保守。它知道所有涉及的类型，并且可以区分看起来像指针的整数和实际的指针值。
//它知道所有堆栈分配的变量，以及这些变量的作用域。最后，GC不能查看非托管代码。
//这样做的结果是GC可以收集类实例
//这怎么可能?如果方法不再引用类数据(实例成员)，并且没有其他代码引用它，则GC可以收集类。
//毕竟，如果没有使用实例成员，也没有人使用实例，那么收集实例又有什么关系呢?
//如果非托管代码认为实例仍然是活的，那就很重要了。或者，如果类有终结器，则可以在本机方法内执行本机代码时执行终结器。
class CppGC
{
    // handle into unmanaged memory, for an unmanaged object
    //IntPtr _handle;

    HandleRef _handle; //使用HandleRef代替intptr

    // performs some operation on h
    [DllImport("...")]
    static extern void OperateOnHandle(HandleRef h);

    // frees resources of h
    [DllImport("...")]
    static extern void DeleteHandle(HandleRef h);

    // Creates Resource
    [DllImport("...")]
    static extern IntPtr CreateHandle();

    public CppGC()
    {
        // _handle = CreateHandle();
        IntPtr h = CreateHandle();
        _handle = new HandleRef(this, h);

    }

    ~CppGC()
    {
        DeleteHandle(_handle);
    }

    public void m()
    {
        OperateOnHandle(_handle);
        // no further references to _handle
    }
}

class Other
{
    //假设我连续两次new ，那么出现问题了，拿到两个指针
    //Other.work
    //CppGC.m
    //CppGC.OperateOnHandle
    //Other.workcc

    //上面代码会出现 C。DeleteHandle   C.OperateOnHandle
    //如何避免这个问题? 不要使用原始IntPtrs。在使用IntPtr时，GC无法知道类是否仍然需要挂起。为了避免错误，我们避免IntPtrs。
    //我们使用IntPtr代替IntPtr
    //接下来，不是让P/Invoke代码接受IntPtr参数，而是让P/Invoke代码接受HandleRefs。HandleRefs对于运行时和GC系统是特殊的，在封送操作期间，它们“折叠”成IntPtr。
    //这允许我们编写安全代码:
    void work()
    {
        CppGC c = new CppGC();
        c.m();
        // no further references to c.
        // c is now eligable for collection.
    }
}
