using System;
using System.Runtime.InteropServices;

namespace LuaInterface
{ 
    // only for lapi.h
    public partial class LuaVMAPI
    {
        //UNITY_WEBGL也使用__Internal
#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_WEBGL)
        const string LUAVM_DLL_NAME = "__Internal";
#else
        const string LUAVM_DLL_NAME = "lua54";
#endif



        #region  state manipulation

        public delegate IntPtr lua_Alloc(IntPtr ud, IntPtr ptr, uint osize, uint nsize);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_newstate(lua_Alloc f, IntPtr ud);  //luajit64位不能用这个函数

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_close(IntPtr luaState);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)] 
        public static extern IntPtr lua_newthread(IntPtr L);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)] 
        public static extern IntPtr lua_closethread(IntPtr L, IntPtr from);


        public delegate int  lua_CFunction (IntPtr L);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_atpanic(IntPtr L, lua_CFunction panicf); 

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double lua_version(IntPtr L);

        #endregion



        #region  basic stack manipulation

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern double lua_absindex(IntPtr L, int idx);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gettop(IntPtr L);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_settop(IntPtr L, int top);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_closeslot(IntPtr L, int idx);
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushvalue(IntPtr L, int idx);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rotate(IntPtr L, int idx, int n);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_copy(IntPtr L, int fromidx, int toidx);



        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_checkstack(IntPtr L, int n);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_xmove(IntPtr from, IntPtr to, int n);


        //[DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void lua_remove(IntPtr L, int idx);


        //[DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void lua_insert(IntPtr L, int idx);


        //[DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        //public static extern void lua_replace(IntPtr luaState, int index);

        #endregion basic stack manipulation





        #region access functions (stack -> C)

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LuaTypes lua_type(IntPtr luaState, int index);

        //---- lua_typename本质上是取类型
        //这里为了提高性能，直接将 类型数组copy到C#这边，直接查询
        static string[] luaT_typenames_ =  new string[12]{
          "no value",
          "nil", "boolean",  "userdata", "number",
          "string", "table", "function", "userdata", "thread",
          "upvalue", "proto" /* these last cases are used for tests only */
        };

        public static string lua_typename(IntPtr luaState, LuaTypes type)
        {
            int t = (int)type;
            if (!(-1 <= t && t < 9))
                return "invalid type";
            return luaT_typenames_[t + 1];
        }


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_iscfunction(IntPtr luaState, int index);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isinteger(IntPtr luaState, int index);



        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isnumber(IntPtr luaState, int idx);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isstring(IntPtr luaState, int index);



        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_isuserdata(IntPtr luaState, int stackPos);



        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_equal(IntPtr luaState, int idx1, int idx2);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_rawequal(IntPtr luaState, int idx1, int idx2);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_arith(IntPtr luaState, int op);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_compare(IntPtr L, int index1, int index2, int op);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lua_stringtonumber(IntPtr L, string index1);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lua_tonumberx(IntPtr L, int idx, IntPtr pisnum);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern long lua_tointegerx(IntPtr L, int idx, IntPtr pisnum); //return long long


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_toboolean(IntPtr L, int idx);

        public static bool luaVM_toboolean(IntPtr L, int idx)
        {
            int value = lua_toboolean(L, idx);
            return value == 0 ? false : true;
        }


        //要处理string转intptr，不然一定崩溃 因为C#的string跟C编码不同
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern string lua_tolstring(IntPtr L, int idx);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern string lua_tolstring(IntPtr L, int idx,out uint len); //len在win64下是unsigned long，在其他平台是 uint


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern uint lua_rawlen(IntPtr L, int idx);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_tocfunction(IntPtr luaState, int idx);



        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_touserdata(IntPtr luaState, int idx);



        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_tothread(IntPtr L, int idx);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_topointer(IntPtr L, int idx);


        #endregion




        //[DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        //public static extern int lua_lessthan(IntPtr luaState, int idx1, int idx2);



        //[DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        //public static extern double lua_tonumber(IntPtr luaState, int idx);

        //public static int lua_tointeger(IntPtr luaState, int idx)
        //{
        //    return tolua_tointeger(luaState, idx);
        //}


        //public static int lua_objlen(IntPtr luaState, int idx)
        //{
        //    return tolua_objlen(luaState, idx);
        //}





        #region push functions (C -> stack)

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushnil(IntPtr luaState);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushnumber(IntPtr luaState, double number);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushinteger(IntPtr luaState, int number);

        //const char *   == string  or  byte[]
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern string lua_pushlstring(IntPtr luaState, byte[] str, int size);


        public static string luaVM_pushlstring(IntPtr luaState, byte[] str, int size)
        {  
            if (size ==0 )
            {
                throw new LuaException("string length is zero");
            }

            if (size >= 0x7fffff00) 
            {
                throw new LuaException("string length overflow");
            }

           return lua_pushlstring(luaState, str, size);
        }

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern string lua_pushstring(IntPtr luaState, string str);


        //argp is char *
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern string lua_pushvfstring(IntPtr luaState, string fmt, string argp);

        //...
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern string lua_pushfstring(IntPtr luaState, string fmt);             


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushcclosure(IntPtr luaState, IntPtr fn, int n);  //lua_CFunction fn,




        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushboolean(IntPtr luaState, int b);

        public static void lua_pushboolean(IntPtr luaState, bool value)
        {
            lua_pushboolean(luaState, value ? 1 : 0);
        }



        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_pushlightuserdata(IntPtr luaState, IntPtr udata);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_pushthread(IntPtr L);


        #endregion


        #region get functions (Lua -> stack)

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_getglobal(IntPtr luaState, string name);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_getglobal(IntPtr luaState, int idx);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_getfield(IntPtr L, int idx, string key);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_geti(IntPtr L, int idx, long n);


        //public static void lua_gettable(IntPtr L, int idx)
        //{
        //    if (LuaDLL.tolua_gettable(L, idx) != 0)
        //    {
        //        string error = LuaDLL.lua_tostring(L, -1);
        //        throw new LuaException(error);
        //    }
        //}


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawget(IntPtr luaState, int idx);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawgeti(IntPtr luaState, int idx, int n);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawgetp(IntPtr luaState, int idx, in IntPtr n);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_createtable(IntPtr luaState, int narr, int nrec);             




        //public static IntPtr lua_newuserdata(IntPtr luaState, int size)                         
        //{
        //    return tolua_newuserdata(luaState, size);
        //}

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_getmetatable(IntPtr luaState, int objIndex);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_getiuservalue(IntPtr luaState, int idx, int n);

        #endregion


        #region set functions (stack -> Lua)

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_setglobal(IntPtr luaState, string name);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_settable(IntPtr L, int idx);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_setfield(IntPtr L, int idx, string key);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_seti(IntPtr L, int idx, long n);



        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawset(IntPtr luaState, int idx);                             //[-2, +0, m]
                                                                                                    //

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawsetp(IntPtr luaState, in IntPtr p);                             //[-2, +0, m]       


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_rawseti(IntPtr luaState, int tableIndex, int index);          //[-1, +0, m]



        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_setmetatable(IntPtr luaState, int objIndex);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_setiuservalue(IntPtr luaState, int idx, int n);

        #endregion

        #region 'load' and 'call' functions (run Lua code)

        public delegate int lua_KFunction(IntPtr L, int status, long ctx);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_callk(IntPtr L, int nargs, int nresults, long ctx, lua_KFunction k);

        /*
         ** Execute a protected call.
        */

        [DllImport("luaCWrap", CallingConvention = CallingConvention.Cdecl)]
        public static extern int luavm_pcall(IntPtr L, int nargs, int nresults, int ctx);


        public delegate string lua_Reader(IntPtr L, IntPtr status, out uint ctx);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_load(IntPtr L, lua_Reader reader, IntPtr data, string chunkname, string mode);


        public delegate int lua_Writer(IntPtr L, in IntPtr p,  uint sz, IntPtr ud);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_dump(IntPtr L, lua_Reader reader, IntPtr data, int strip);




        #endregion


        #region coroutine functions

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_yield(IntPtr L, int nresults);                                 //[-?, +?, e]       
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_resume(IntPtr L, int narg);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_status(IntPtr L);
        #endregion

        #region garbage-collection function and options
        public enum LuaVM_GCOptions
        {
            LUA_GCSTOP = 0,
            LUA_GCRESTART = 1,
            LUA_GCCOLLECT = 2,
            LUA_GCCOUNT = 3,
            LUA_GCCOUNTB = 4,
            LUA_GCSTEP = 5,
            LUA_GCSETPAUSE = 6,
            LUA_GCSETSTEPMUL = 7,
            LUA_GCISRUNNING = 9,
            LUA_GCGEN = 10,
            LUA_GCINC = 11,
        }
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gc(IntPtr luaState, LuaVM_GCOptions what, int data);              //[-0, +0, e]

        #endregion



        #region  miscellaneous functions
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_error(IntPtr luaState);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_next(IntPtr luaState, int index);                              //[-1, +(2|0), e]
                                                                                                    //
        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_toclose(IntPtr luaState, int idx);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_concat(IntPtr luaState, int n);                               //[-n, +1, e]


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_len(IntPtr luaState, int idx);                               //[-n, +1, e]

        // [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        // public static extern void lua_getallocf(IntPtr luaState, int n);          //LUA_API lua_Alloc lua_getallocf (lua_State *L, void **ud) 

        //LUA_API void lua_setallocf (lua_State *L, lua_Alloc f, void *ud) {

        //LUA_API void *lua_newuserdatauv (lua_State *L, size_t size, int nuvalue)

        //LUA_API const char *lua_getupvalue (lua_State *L, int funcindex, int n)

        //LUA_API const char *lua_setupvalue (lua_State *L, int funcindex, int n)

        //LUA_API void *lua_upvalueid (lua_State *L, int fidx, int n)

        //LUA_API void lua_upvaluejoin (lua_State *L, int fidx1, int n1,int fidx2, int n2) 

        #endregion




        #region some useful macros



        #endregion


        /*
         ** ======================================================================
         ** Debug API
         ** =======================================================================
         */

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_getstack(IntPtr L, int level, ref Lua_Debug ar);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_getinfo(IntPtr L, string what, ref Lua_Debug ar);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern string lua_getlocal(IntPtr L, ref Lua_Debug ar, int n);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern string lua_setlocal(IntPtr L, ref Lua_Debug ar, int n);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern string lua_getupvalue(IntPtr L, int funcindex, int n);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern string lua_setupvalue(IntPtr L, int funcindex, int n);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr lua_upvalueid (IntPtr L, int fidx, int n);

        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern void lua_upvaluejoin (IntPtr L, int fidx1, int n1, int fidx2, int n2);



        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_sethook(IntPtr L, LuaHookFunc func, int mask, int count);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern LuaHookFunc lua_gethook(IntPtr L);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gethookmask(IntPtr L);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern int lua_gethookcount(IntPtr L);


        [DllImport(LUAVM_DLL_NAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern  int lua_setcstacklimit (IntPtr L, uint limit);

    }
}