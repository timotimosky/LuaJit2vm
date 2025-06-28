using LuaInterface;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallLuaTest : MonoBehaviour
{
    System.IntPtr L;
    void Start()
    {
        // 1~3������ջ��Ȼ��������Դ��
        //1�½������  
        L = LuaVMAPI.luaL_newstate();
        //2�����  
        LuaVMAPI.luaL_openlibs(L);


        string luafile =
          @"  
            version = 'luavm 5.4.6'
            function luaFunc(a,b)                   
                 return a+b,999
            end
        ";

        LuaVMAPI.DoString(L, luafile);
        //4��������ջ�ף�������̣���Ϊ��ȷ��ջ���ǿյģ��Ա����Ĳ����ǰ���˳����ջ���Ҵ�1��ջλ��ʼ
        LuaVMAPI.lua_settop(L, 0);

        // get global variable
        LuaVMAPI.lua_getglobal(L, "version");
      
        string version = LuaVMAPI.lua_tostring(L, -1); //��ջ����ȡ
        Debug.Log("version is "+ version);
       // return 0;

        //C#����lua ����LuaFunc"�ŵ�ջ��
        LuaVMAPI.lua_getglobal(L, "luaFunc");
        LuaVMAPI.lua_pushinteger(L, 12);
        LuaVMAPI.lua_pushinteger(L, 52);

        //luavm_pcall ����luaջ���ĺ���
        //4������
        //����1: L
        // ����2: "��������2��������
        //����3: �᷵��2�������� a+b����ջ�ף�999����ջ
        // ����4: ����Ҫ������Ϣ��0��
        LuaVMAPI.luavm_pcall(L, 2, 2, 0);

        //luavm_pcallִ�к󣬻Ὣ����ŵ�ջ��

        //��ӡջ���Ԫ��,�������򶼿���
        Debug.Log(LuaVMAPI.lua_tostring(L, -1));//lua_tostring(L,1)�Ƕ�ȡ����,����ɾ��ջ�ڶ�ȡ����Ԫ��
        Debug.Log(LuaVMAPI.lua_tostring(L, -2));
        Debug.Log(LuaVMAPI.lua_tostring(L, -3));

        Debug.Log(LuaVMAPI.lua_tostring(L, 1));
        Debug.Log(LuaVMAPI.lua_tostring(L, 2));
        Debug.Log(LuaVMAPI.lua_tostring(L, 3));

        //��ʹ��lua_pop��L,1�� ȥ�����Ļ������Ե���ָ����λ�õ�ջ����


        //7һ���ǵ��ֶ��ر������ 
        LuaVMAPI.lua_close(L);
    }

}
