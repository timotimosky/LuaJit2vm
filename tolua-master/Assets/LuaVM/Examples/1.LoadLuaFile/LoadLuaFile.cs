using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.IO;

//展示searchpath 使用，require 与 dofile 区别
public class LoadLuaFile : MonoBehaviour
{
    LuaVMState lua = null;
    private string strLog = "";

    void Start()
    {
        Application.logMessageReceived += Log;
        lua = new LuaVMState();
        lua.InitLuaState();

        string fullPath = Application.dataPath + "\\LuaVM/Examples/1.LoadLuaFile";
        Debug.Log(fullPath);
        lua.AddSearchPath(fullPath);
    }

    void Log(string msg, string stackTrace, LogType type)
    {
        strLog += msg;
        strLog += "\r\n";
    }

    void OnGUI()
    {
        GUI.Label(new Rect(100, Screen.height / 2 - 100, 600, 400), strLog);

        if (GUI.Button(new Rect(50, 50, 120, 45), "DoFile"))
        {
            strLog = "";
            lua.DoFile("ScriptsFromFile.lua");
        }
        else if (GUI.Button(new Rect(50, 150, 120, 45), "Require"))
        {
            strLog = "";
            lua.Require("ScriptsFromFile");
        }

        //lua.Collect();
       // lua.CheckTop();
    }

    void OnApplicationQuit()
    {
        lua.Dispose();
        lua = null;
        Application.logMessageReceived -= Log;
    }
}
