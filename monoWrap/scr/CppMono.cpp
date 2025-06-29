// MonoDemo.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include <iostream>

#include <mono/jit/jit.h>
#include <mono/metadata/assembly.h>
#include <mono/metadata/class.h>
#include <mono/metadata/debug-helpers.h>
#include <mono/metadata/mono-config.h>

MonoDomain* domain;

int main()
{
    // Program.cs所编译dll所在的位置
   // const char* managed_binary_path = "CSharp.dll";
    const char* managed_binary_path = "D:/Program.dll";

    //获取应用域
    domain = mono_jit_init("Test");

    //加载程序集ManagedLibrary.dll
    MonoAssembly* assembly = mono_domain_assembly_open(domain, managed_binary_path);
    MonoImage* image = mono_assembly_get_image(assembly);

    // =====================================================准备调用
    //获取MonoClass,类似于反射
    MonoClass* main_class = mono_class_from_name(image, "MonoCsharp", "MainTest");

    //获取要调用的MonoMethodDesc,主要调用过程
    MonoMethodDesc* entry_point_method_desc = mono_method_desc_new("MonoCsharp.MainTest:Main()", true);
    MonoMethod* entry_point_method = mono_method_desc_search_in_class(entry_point_method_desc, main_class);
    mono_method_desc_free(entry_point_method_desc);
    //调用方法
    mono_runtime_invoke(entry_point_method, NULL, NULL, NULL);
    //释放应用域
    mono_jit_cleanup(domain);

    return 0;
}