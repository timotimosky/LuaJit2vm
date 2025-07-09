using AOT;
using LuaInterface;
using NUnit.Framework.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public class LuaCallUnity : MonoBehaviour
{
    System.IntPtr L;
    delegate void UnityFunc();
    [AOT.MonoPInvokeCallback(typeof(UnityFunc))]
    static void OnDebug(string log)
    {
        Debug.Log(log);
    }

    void Start()
    {
        OpenLibs();
    }

    public static void OpenLibs()
    {
        IntPtr  L = LuaVMAPI.luaL_newstate();
        LuaVMAPI.luaL_openlibs(L);
        //......
        //IntPtr fn = Marshal.GetFunctionPointerForDelegate(Print);
        LuaVMAPI.lua_pushcfunction(L, Print);
        // LuaDLL.tolua_pushcfunction(L, Print);

        //将栈顶元素赋值给name变量。(name参数的值，是lua脚本中全部变量的名字。)
        //也就是以后 lua代码中的"print"，对应着c#中的Print函数的指针
        LuaVMAPI.luavm_setglobal(L, "print");
       // LuaDLL.lua_setglobal
      //  LuaVMAPI.lua_setglobal(L, "print");
        //......

        string luafile =
         @"  
            print(""Hello, World!"")  -- 输出: Hello, World!
        ";
        LuaVMAPI.DoString(L, luafile);
    }

    [AOT.MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
    static int Print(IntPtr L)
    {
        try
        {
            int n = LuaDLL.lua_gettop(L);

            using (CString.Block())
            {
                CString sb = CString.Alloc(256);
#if UNITY_EDITOR
                //获得当前运行的函数的上一个调用层的信息，返回行数，把调用层的名称入栈
                int line = LuaDLL.tolua_where(L, 1);
                string filename = LuaDLL.lua_tostring(L, -1);
                LuaDLL.lua_settop(L, n);
                int offset = filename[0] == '@' ? 1 : 0;

                if (!filename.Contains("."))
                {
                    sb.Append('[').Append(filename, offset, filename.Length - offset).Append(".lua:").Append(line).Append("]:");
                }
                else
                {
                    sb.Append('[').Append(filename, offset, filename.Length - offset).Append(':').Append(line).Append("]:");
                }
#endif

                for (int i = 1; i <= n; i++)
                {
                    if (i > 1) sb.Append("    ");

                    if (LuaDLL.lua_isstring(L, i) == 1)
                    {
                        sb.Append(LuaDLL.lua_tostring(L, i));
                    }
                    else if (LuaDLL.lua_isnil(L, i))
                    {
                        sb.Append("nil");
                    }
                    else if (LuaDLL.lua_isboolean(L, i))
                    {
                        sb.Append(LuaDLL.lua_toboolean(L, i) ? "true" : "false");
                    }
                    else
                    {
                        IntPtr p = LuaDLL.lua_topointer(L, i);

                        if (p == IntPtr.Zero)
                        {
                            sb.Append("nil");
                        }
                        else
                        {
                            sb.Append(LuaDLL.luaL_typename(L, i)).Append(":0x").Append(p.ToString("X"));
                        }
                    }
                }

                Debugger.Log(sb.ToString());            //203行与_line一致
            }
            return 0;
        }
        catch (Exception e)
        {
            return LuaDLL.toluaL_exception(L, e);
        }
    }



    [DllImport("__Internal")]
    static extern void RegisterCallback(UnityFunc func);
    //接受回调的 C 代码如下所示
    // 注意： 确保从原生方法返回的字符串值是 UTF-8 编码的，并在堆上分配。
    //typedef void (* UnityFunc) ();
    //void RegisterCallback(UnityFunc func) { }



}
