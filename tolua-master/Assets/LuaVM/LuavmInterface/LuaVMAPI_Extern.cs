using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace LuaInterface
{
    public partial class LuaVMAPI
    {
        //加载并运行指定的字符串
        //如果没有错误，函数返回假；有错则返回真。
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luavm_tostring(IntPtr L, string s);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int dostring(IntPtr L, string s);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr luavm_tolstring(IntPtr L, int index, out int len);


        //适配函数
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luavm_loadbuffer(IntPtr luaState, byte[] buff, int size, string name);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int luavm_pushcfunction(IntPtr luaState, IntPtr fn);

        public static void DoString(IntPtr L, string chunk)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(chunk);
            LuaLoadBuffer( L, buffer);
        }


        public static void lua_dostring(IntPtr luaState, string chunk)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(chunk);

           // IntPtr init = Marshal.StringToHGlobalAnsi(chunk);

          //  lua_ptrtostring(init, chunkName);
        }


#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_WSA_10_0
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int LuaCSFunction(IntPtr luaState);
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LuaHookFunc(IntPtr L, ref Lua_Debug ar);
#else
    public delegate int LuaCSFunction(IntPtr luaState);    
    public delegate void LuaHookFunc(IntPtr L, ref Lua_Debug ar);    
#endif

        public static void pushcfunction(IntPtr luaState, LuaCSFunction func)
        {
            IntPtr fn = Marshal.GetFunctionPointerForDelegate(func);
            luavm_pushcfunction(luaState, fn);
        }


        //真正给外部调用的
        public static bool luaVM_dostring(IntPtr luaState, string chunk)
        {
            int result = dostring(luaState, chunk);

            if (result != 0)
            {
                UnityEngine.Debug.LogError("lua chunk is not exist!");
                return false;
            }
            return true;
            //return lua_pcall(luaState, 0, LUA_MULTRET, 0) == 0;
        }

        //真正给外部调用的 [1-n]
        public static string lua_tostring(IntPtr luaState, int index)
        {
            int len = 0;
            //从C中获得len
            IntPtr str = luavm_tolstring(luaState, index, out len);

            if (str != IntPtr.Zero)
            {
                return lua_ptrtostring(str, len);
            }

            return null;
        }

        //不可以直接用StringBuilder或者string接收const char*
        //因为C的string结尾符不同
        //1.将const char*转为intptr返回，同时返回string长度。
        // 2. IntPtr转换为string–>调用Marshal.PtrToStringAnsi()
        public static string lua_ptrtostring(IntPtr str, int len)
        {
            string ss = Marshal.PtrToStringAnsi(str, len);

            if (ss == null)
            {
                UnityEngine.Debug.Log("Marshal.Copy str");
                byte[] buffer = new byte[len];
                Marshal.Copy(str, buffer, 0, len);
                return Encoding.UTF8.GetString(buffer);
            }

            return ss;
        }

        protected static void LuaLoadBuffer(IntPtr L, byte[] buffer)
        {
            //LuaVMAPI.tolua_pushtraceback(L);

            //执行之前，记录栈顶
            int oldTop = LuaVMAPI.lua_gettop(L);

            //if (LuaVMAPI.luaL_loadstring(L, buffer) == 0)


          //  if (LuaDLL.tolua_loadbuffer(L, buffer, buffer.Length, chunkName) == 0)
            if (LuaVMAPI.luavm_loadbuffer(L, buffer, buffer.Length, "aa") == 0)
            {
                //将程序块从栈中弹出，并在保护模式下运行该程序块。执行成功返回0，否则将错误信息压入栈中。
                //为什么成功了，还要栈顶-1?

                if (LuaVMAPI.luavm_pcall(L, 0, LuaDLL.LUA_MULTRET, oldTop) == 0)
                {
                    //恢复栈顶，因为
                    LuaVMAPI.lua_settop(L, oldTop - 1);
                    return;
                }
            }

            string err = LuaVMAPI.lua_tostring(L, -1);
            LuaVMAPI.lua_settop(L, oldTop - 1);
            throw new LuaException(err, LuaException.GetLastError());
        }
    }
}
