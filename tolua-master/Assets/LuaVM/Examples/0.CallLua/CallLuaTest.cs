using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallLuaTest : MonoBehaviour
{
    System.IntPtr L;
    void Start()
    {
        // 1~3：建立栈，然后载入资源；
        //1新建虚拟机  
        L = LuaVMAPI.luaL_newstate();
        //2载入库  
        LuaVMAPI.luaL_openlibs(L);


        string luafile =
          @"  
            version = 'luavm 5.4'
            function luaFunc()                   
                 return a,b,c,d
            end
        ";

        //3这里执行 test.lua  Lua文件  
        // LuaVMAPI.luaVM_dostring(L, "luafile.lua");
        //bool dostringReselt = LuaVMAPI.luaVM_dostring(L, luafile);

        //if (!dostringReselt)
        //{
        //    //7一定记得手动关闭虚拟机 
        //    LuaVMAPI.lua_close(L);
        //    return 0;
        //}
        LuaVMAPI.DoString(L, luafile);
        //4重新设置栈底：这个过程，是为了确认栈底是空的，以便后面的操作是按照顺序入栈的且从1号栈位开始
        LuaVMAPI.lua_settop(L, 0);


        // get global variable
        LuaVMAPI.lua_getglobal(L, "version");
      
        string version = LuaVMAPI.lua_tostring(L, -1); //从栈顶获取
        Debug.Log("version is "+ version);
       // return 0;

        //5获取返回结果：这步开始 C++去访问虚拟机的栈，送 “LuaFunc"入栈
        LuaVMAPI.lua_getglobal(L, "luaFunc");


        //1>  C++告诉Lua虚拟机（L)，已将函数“LuaFunc"加载入栈（栈中一个元素：“LuaFunc" ）
        // 参数2: "函数传入0个参数，
        //参数3: 会返回4个参数，
        // 参数4: 不需要错误信息（0）
        //2> 这里，C++（宿主语言）请求完毕了，虚拟机（L）开始访问栈，从栈中取出“LuaFunc"。（栈中无元素了：null）
        //3> 虚拟机得到 “LuaFunc" 信息送给 Lua程序（编译器）。（栈中无元素了：null）
        //4> Lua程序 在调用的Lua文件全局表（Global table）中查找 “LuaFunc" ，并运行返回结果“a,b,c,d”。（栈中无元素了：null）
        //5> Lua程序得到返回结果“a,b,c,d” 将结果再压入栈；压入顺序为，顺序的，“a”先入栈底，“b”再入栈，以此类推。（栈中四个元素：a,b,c,d）（顺序为栈底->栈顶）
        //6>  最后，C++（宿主语言）再去栈中读取数据；这里 lua_tostring(L,1)是读取函数，不会改变栈内的结果的，所以当地⑥步执行完，栈中还是四个元素：a,b,c,d
        //提示：若使用lua_pop（L,1） 去操作的话，可以弹出指定的位置的栈内容

        //6操作栈调回结果           
        LuaVMAPI.lua_pcallk(L, 0, 4, 0);

        //打印栈里的元素
        Debug.Log( LuaVMAPI.lua_tostring(L, 1));
        Debug.Log(LuaVMAPI.lua_tostring(L, 2));
        Debug.Log(LuaVMAPI.lua_tostring(L, 3));
        Debug.Log(LuaVMAPI.lua_tostring(L, 4));
        Debug.Log(LuaVMAPI.lua_tostring(L, 5));
        //7一定记得手动关闭虚拟机 
        LuaVMAPI.lua_close(L);
    }

}
