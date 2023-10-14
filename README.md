# LuaJit2vm
LuaJit 替換為luavm

步骤：

1.替换tolua虚拟机为lua5.3

2.修改部分tolua不兼容代码

3.修改回调为_internal

#if !UNITY_EDITOR && (UNITY_IPHONE || UNITY_WEBGL)

4.源码直接放到插件目录的webgl目录下。
