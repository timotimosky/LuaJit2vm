using fts;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;

//基于DllLoader，实现所有DLL的全局的管理：
//1.动态加载和卸载
public class DllManager : MonoBehaviour
{
    static Dictionary<string, IntPtr> _loadedPlugins = new Dictionary<string, IntPtr>();

    // Load all plugins with 'PluginAttr'
    // Load all functions with 'PluginFunctionAttr'
    public static void LoadAllDll()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        foreach (var assembly in assemblies)
        {
            // Loop over all types
            foreach (var type in assembly.GetTypes())
            {
                SetDelegate(type);
            }
        }
    }

    public static void UnloadAll()
    {
        foreach (var kvp in _loadedPlugins)
        {
            DllLoader.FreeLib(kvp.Value);
        }
        _loadedPlugins.Clear();
    }

    static void SetDelegate(Type type)
    {
        // Get custom attributes for type
        var typeAttributes = type.GetCustomAttributes(typeof(NativeDLLAttribute), true);
        if (typeAttributes.Length == 0)
        {
            return;
        }

        var typeAttribute = typeAttributes[0] as NativeDLLAttribute;

        var dllName = typeAttribute.pluginName;

        if (!_loadedPlugins.TryGetValue(dllName, out IntPtr dllHandle))
        {
            dllHandle = DllLoader.LoadDll(dllName);
            _loadedPlugins.Add(dllName, dllHandle);
        }

        // Loop over fields in type
        FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
        foreach (var field in fields)
        {
            var fieldAttributes1 = field.GetCustomAttributes(typeof(NativeDLLFunctionAttribute), true);
            Debug.Log("在当前类查询到的方法数量="+fieldAttributes1.Length);
            if (fieldAttributes1.Length == 0)
                continue;

            // Get function pointer
            var fnPtr = DllLoader.GetFuncPtrCrossPlatform(dllHandle, field.Name);
            if (fnPtr == IntPtr.Zero)
                continue;

            // Get delegate pointer
            Delegate fnDelegate = Marshal.GetDelegateForFunctionPointer(fnPtr, field.FieldType);

            // Set static field value
            field.SetValue(null, fnDelegate);
        }
    }

}
