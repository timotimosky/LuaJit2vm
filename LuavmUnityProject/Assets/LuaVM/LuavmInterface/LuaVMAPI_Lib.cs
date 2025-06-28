using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
namespace LuaInterface
{
    //for Lauxlib.h
    public partial class LuaVMAPI
    {
        public static int LUA_MULTRET = -1;

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern System.IntPtr luaL_newstate();


        /* open all previous libraries */
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void luaL_openlibs(IntPtr L);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_loadfile(IntPtr luaState, string filename);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void openVMint64(System.IntPtr a);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luaL_loadstring(IntPtr luaState, string chunk);
    }
}