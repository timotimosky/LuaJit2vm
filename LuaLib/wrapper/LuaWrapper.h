#pragma once
#include <widemath.h>
#include "../src/lua.h"
#include "../src/lualib.h"
#include "../src/lauxlib.h"

//#include <cstdint> 
#define LUA_RIDX_LOADED				26
#define LUA_LIB

#if defined(LUA_BUILD_AS_DLL)

   #if defined(LUA_CORE) || defined(LUA_LIB)
      #define LUA_API __declspec(dllexport)
   #else
      #define LUA_API __declspec(dllimport)
   #endif
#else
    #define LUA_API		extern //lib 会提供h文件只需要 extern
#endif


void openVMint64(lua_State* L);


LUALIB_API int luavm_loadbuffer(lua_State* L, const char* buff, int sz, const char* name);

LUA_API int dostring(lua_State* L, const char* s);

LUA_API int luavm_tostring(lua_State* L, const char* o);
