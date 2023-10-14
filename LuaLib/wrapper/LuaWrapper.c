#include "LuaWrapper.h"
#include "../src/llex.h"

//#include "../src/int64.h"

#define lua_getref(L,ref)       lua_rawgeti(L, LUA_REGISTRYINDEX, (ref))


LUA_API const char* luavm_tolstring(lua_State* L, int index, int* len)
{
    size_t sz;
    const char* ret = lua_tolstring(L, index, &sz);
    *len = (int)sz;
    return ret;
}

LUALIB_API int luavm_loadbuffer(lua_State* L, const char* buff, int sz, const char* name)
{
    return luaL_loadbuffer(L, buff, (size_t)sz, name);
}


LUA_API int dostring(lua_State* L, const char* s)
{
    return luaL_dostring(L, s);
}

LUA_API int luavm_tostring(lua_State* L, const char* o)
{
    return 	(luaL_loadstring(L, o) || lua_pcall(L, 0, LUA_MULTRET, 0));
}


int luaVM_dostring(lua_State* L, int index, int* len)
{
    size_t sz;
    const char* ret = lua_tolstring(L, index, &sz);
    *len = (int)sz;
    return ret;
}

void openVMint64(lua_State* L)
{
    lua_newtable(L);
    lua_pushvalue(L, -1);
    lua_setglobal(L, "int64");

    lua_getref(L, LUA_RIDX_LOADED);
    lua_pushstring(L, "int64");
    lua_pushvalue(L, -3);
    lua_rawset(L, -3);
    lua_pop(L, 1);

    //lua_pushstring(L, "__add"),
    //    lua_pushcfunction(L, _int64add);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "__sub"),
    //    lua_pushcfunction(L, _int64sub);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "__mul"),
    //    lua_pushcfunction(L, _int64mul);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "__div"),
    //    lua_pushcfunction(L, _int64div);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "__mod"),
    //    lua_pushcfunction(L, _int64mod);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "__unm"),
    //    lua_pushcfunction(L, _int64unm);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "__pow"),
    //    lua_pushcfunction(L, _int64pow);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "__tostring");
    //lua_pushcfunction(L, _int64tostring);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "tostring");
    //lua_pushcfunction(L, _int64tostring);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "__eq");
    //lua_pushcfunction(L, _int64eq);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "__lt");
    //lua_pushcfunction(L, _int64lt);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "__le");
    //lua_pushcfunction(L, _int64le);
    //lua_rawset(L, -3);

    //lua_pushstring(L, ".name");
    //lua_pushstring(L, "int64");
    //lua_rawset(L, -3);

    //lua_pushstring(L, "new");
    //lua_pushcfunction(L, tolua_newint64);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "equals");
    //lua_pushcfunction(L, _int64equals);
    //lua_rawset(L, -3);

    //lua_pushstring(L, "tonum2");
    //lua_pushcfunction(L, _int64tonum2);
    //lua_rawset(L, -3);

    lua_pushstring(L, "__index");
    lua_pushvalue(L, -2);
    lua_rawset(L, -3);

    //lua_rawseti(L, LUA_REGISTRYINDEX, LUA_RIDX_INT64);
}

int pushInt2VM1()
{
    ///* ��ʼ�� */
    //lua_State* pL = luaL_newstate();
    //luaopen_base(pL);

    ///* ִ�нű� */
    //luaL_dofile(pL, "helloLua.lua");
    ///* ȡ��table��������ջ�� */
    //lua_getglobal(pL, "helloTable");
    ///* ��C++���ַ����ŵ�Lua��ջ�У���ʱ��ջ����Ϊ��name���� helloTable�����Ϊջ�� */
    //lua_pushstring(pL, "name");

    ////��table����Ѱ�ҡ�name����Ӧ��ֵ��table��������������Ϊ-2��ջ�У�Ҳ���ǵ�ǰ��ջ�ף�,ȡ�ö�Ӧֵ֮�󣬽�ֵ�Ż�ջ��
    //lua_gettable(pL, -2);
    ///* ���ڱ��name��Ӧ��ֵ�Ѿ���ջ���ˣ�ֱ��ȡ������ */
    //const char* sName = lua_tostring(pL, -1);
    ////printf("name = %s", sName);
    return 1;
}

__declspec(dllexport) int pushInt2VM2()
{
    return 2;
}