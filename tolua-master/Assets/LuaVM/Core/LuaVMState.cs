using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class LuaVMState : MonoBehaviour
{
    System.IntPtr L;

    void Start()
    {

    }

    public void InitLuaState()
    {
        //1新建虚拟机  
        L = LuaVMAPI.luaL_newstate();
        //2载入库  
        LuaVMAPI.luaL_openlibs(L);
    }


    public void DoFile(string fileName)
    {
        byte[] buffer = LoadFileBuffer(fileName);
        fileName = LuaChunkName(fileName);
        LuaLoadBuffer(buffer, fileName);
    }

    byte[] LoadFileBuffer(string fileName)
    {
        byte[] buffer = LuaFileUtils.Instance.ReadFile(fileName);

        if (buffer == null)
        {
            string error = string.Format("cannot open {0}: No such file or directory", fileName);
            error += LuaFileUtils.Instance.FindFileError(fileName);
            throw new LuaException(error);
        }

        return buffer;
    }

    string LuaChunkName(string name)
    {
        if (LuaConst.openLuaDebugger)
        {
            name = LuaFileUtils.Instance.FindFile(name);
        }

        return "@" + name;
    }

    public void AddSearchPath(string fullPath)
    {
        if (!Path.IsPathRooted(fullPath))
        {
            throw new LuaException(fullPath + " is not a full path");
        }

        fullPath = ToPackagePath(fullPath);
        LuaFileUtils.Instance.AddSearchPath(fullPath);
    }

    string ToPackagePath(string path)
    {
        using (CString.Block())
        {
            CString sb = CString.Alloc(256);
            sb.Append(path);
            sb.Replace('\\', '/');

            if (sb.Length > 0 && sb[sb.Length - 1] != '/')
            {
                sb.Append('/');
            }

            sb.Append("?.lua");
            return sb.ToString();
        }
    }


    public void DoString(string chunk, string chunkName = "LuaState.cs")
    {
        byte[] buffer = Encoding.UTF8.GetBytes(chunk);
        LuaLoadBuffer(buffer, chunkName);
    }
    protected void LuaLoadBuffer(byte[] buffer, string chunkName)
    {
        // LuaDLL.tolua_pushtraceback(L);
        int oldTop = LuaGetTop();

        if (LuaLoadBuffer(buffer, buffer.Length, chunkName) == 0)
        {
            if (LuaPCall(0, LuaDLL.LUA_MULTRET, oldTop) == 0)
            {
                LuaSetTop(oldTop - 1);
                return;
            }
        }

        string err = LuaVMAPI.lua_tostring(L, -1);
        LuaSetTop(oldTop - 1);
        // throw new LuaException(err, LuaException.GetLastError());
    }

    public int LuaLoadBuffer(byte[] buff, int size, string name)
    {
        return LuaVMAPI.luavm_loadbuffer(L, buff, size, name);
    }

    public bool CheckTop()
    {
        int n = LuaGetTop();

        if (n != 0)
        {
            Debugger.LogWarning("Lua stack top is {0}", n);
            return false;
        }

        return true;
    }

    public int LuaGetTop()
    {
        return LuaVMAPI.lua_gettop(L);
    }

    public string LuaToString(int index)
    {
        return LuaVMAPI.lua_tostring(L, index);
    }

    public void LuaSetTop(int newTop)
    {
        LuaVMAPI.lua_settop(L, newTop);
    }

    public int LuaPCall(int nArgs, int nResults, int errfunc)
    {
        return LuaVMAPI.lua_pcallk(L, nArgs, nResults, errfunc);
    }

    public void Dispose()
    {
        if (IntPtr.Zero != L)
        {
            LuaVMAPI.lua_close(L);
            L = IntPtr.Zero;
            System.GC.SuppressFinalize(this);
        }
    }
}
