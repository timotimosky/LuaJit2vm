# LuaJit2vm
LuaJit 替换为 luavm5.4.6


LuaLib文件夹是luavm的vs工程，双击LuaLib.vcxproj即可运行打包dll。
1.LuaLib\src文件夹： luavm源码
2.LuaLib\wrapper文件夹： 将luavm的函数导出的封装
   绝大部分函数，不需要额外封装了，直接使用luavm的即可。少部分涉及string size_t等类型的做封装。


Assets\LuaVM 文件夹下 是luavm的c#代码
1.Assets\LuaVM\LuavmInterface 是对luavm的封装
   1.1 LuaVMAPI.cs 是对luavm中的lapi.h的封装
   1.2 LuaVMAPI_Lib.cs 是对luavm中的Lauxlib.h函数的封装
   1.3 LuaVMAPI_Extern.cs 是对luavm中的lapi.h之外其他零散函数的封装

2.Assets\LuaVM\Examples
    测试用例，跟tolua保持一致

步骤：

1.替换tolua虚拟机为luavm5.4.6

2.修改部分tolua不兼容代码

3.修改回调为_internal

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_WEBGL)

4.源码直接放到插件目录的webgl目录下。
