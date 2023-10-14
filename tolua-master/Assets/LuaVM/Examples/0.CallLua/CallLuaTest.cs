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
            version = 'luavm 5.4.6'
            function luaFunc(a,b)                   
                 return a+b,999
            end
        ";

        LuaVMAPI.DoString(L, luafile);
        //4重新设置栈底：这个过程，是为了确认栈底是空的，以便后面的操作是按照顺序入栈的且从1号栈位开始
        LuaVMAPI.lua_settop(L, 0);

        // get global variable
        LuaVMAPI.lua_getglobal(L, "version");
      
        string version = LuaVMAPI.lua_tostring(L, -1); //从栈顶获取
        Debug.Log("version is "+ version);
       // return 0;

        //C#告诉lua 将“LuaFunc"放到栈顶
        LuaVMAPI.lua_getglobal(L, "luaFunc");
        LuaVMAPI.lua_pushinteger(L, 12);
        LuaVMAPI.lua_pushinteger(L, 52);

        //luavm_pcall 调用lua栈顶的函数
        //4个参数
        //参数1: L
        // 参数2: "函数传入2个参数，
        //参数3: 会返回2个参数， a+b先入栈底，999再入栈
        // 参数4: 不需要错误信息（0）
        LuaVMAPI.luavm_pcall(L, 2, 2, 0);

        //luavm_pcall执行后，会将结果放到栈顶

        //打印栈里的元素,正序逆序都可以
        Debug.Log(LuaVMAPI.lua_tostring(L, -1));//lua_tostring(L,1)是读取函数,不会删除栈内读取过的元素
        Debug.Log(LuaVMAPI.lua_tostring(L, -2));
        Debug.Log(LuaVMAPI.lua_tostring(L, -3));

        Debug.Log(LuaVMAPI.lua_tostring(L, 1));
        Debug.Log(LuaVMAPI.lua_tostring(L, 2));
        Debug.Log(LuaVMAPI.lua_tostring(L, 3));

        //若使用lua_pop（L,1） 去操作的话，可以弹出指定的位置的栈内容


        //7一定记得手动关闭虚拟机 
        LuaVMAPI.lua_close(L);
    }

}
