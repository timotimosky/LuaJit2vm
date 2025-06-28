// CppInvokeLua.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//
#include <iostream>  
 extern "C" {
 #include "lua.h"
 #include "lualib.h"
 #include "lauxlib.h"
#include "LuaWrapper.h"
 }

 void  doLuaString()
 {
     lua_State* L = luaL_newstate();
     // lua加载字符，生成lua全局函数LuaCode_MyAdd
     //luaL_dostring
     luavm_tostring(L, "function Add (x,y) return x+y end");
     // lua栈和压入数据
     lua_getglobal(L, "Add");
     lua_pushinteger(L, 111);
     lua_pushinteger(L, 222);
     // C调用lua中的函数，2个传入参数，1个返回参数
     lua_call(L, 2, 1);
     std::cout << "lua function ret:" << lua_tointegerx(L, (-1), NULL) ;
     // 栈回到原始状态
     lua_pop(L, 1);
     lua_close(L);
 }

 void doluaFile()
 {
     //1新建虚拟机  
     lua_State* L = luaL_newstate();
     //2载入库  
     luaL_openlibs(L);

     //3执行 test.lua  Lua文件  
     luaL_dofile(L, "luafile.lua");

     //4重新设置栈底：这个过程，是为了确认栈底是空的，以便后面的操作是按照顺序入栈的且从1号栈位开始
     lua_settop(L, 0);

     //5获取返回结果：这步开始 C++去访问虚拟机的栈，送 “LuaFunc"入栈
     lua_getglobal(L, "LuaFunc");

     //6操作栈调回结果           
     lua_pcall(L, 0, 4, 0);
     //打印栈里的元素
     printf("%s\n", lua_tostring(L, 1));
     printf("%s\n", lua_tostring(L, 2));
     printf("%s\n", lua_tostring(L, 3));
     printf("%s\n", lua_tostring(L, 4));

     //7一定记得手动关闭虚拟机 
     lua_close(L);
 }


int main()
{
    std::cout << "Hello Lua!\n"
    doLuaString();
    doluaFile();
    system("pause");
    return 0;
}