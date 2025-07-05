#define UseDynamicDLL
using fts;
using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;


namespace LuaInterface
{


    //for Lauxlib.h
    [NativeDLL(LUAVM_DLL_NAME)]
    public partial class LuaVMAPI
    {
        public static int LUA_MULTRET = -1;

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr luaL_newstate();



        OpenlibDelegate Init;
        public delegate void OpenlibDelegate(IntPtr L);
#if UseDynamicDLL
        //第二种方式:使用反射注解
        [NativeDLLFunctionAttribute]
        public static OpenlibDelegate luaL_openlibs;
#else
        /* open all previous libraries */
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_openlibs(IntPtr L);
        //最简单的..动态加载函数
        public static void luaL_openlibs_new(IntPtr L)
        {
            IntPtr libarayHandle = DllLoader.LoadDll(LUAVM_DLL_NAME);
            OpenlibDelegate Init = DllLoader.GetDelegate<OpenlibDelegate>(libarayHandle, "luaL_openlibs");
            Init(L);
        }
#endif




        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_loadfile(IntPtr luaState, string filename);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void openVMint64(System.IntPtr a);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_loadstring(IntPtr luaState, string chunk);
    }
}