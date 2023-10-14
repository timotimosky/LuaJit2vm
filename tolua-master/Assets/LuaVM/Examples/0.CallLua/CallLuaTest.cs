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
            version = 'luavm 5.4'
            function luaFunc()                   
                 return a,b,c,d
            end
        ";

        //3����ִ�� test.lua  Lua�ļ�  
        // LuaVMAPI.luaVM_dostring(L, "luafile.lua");
        //bool dostringReselt = LuaVMAPI.luaVM_dostring(L, luafile);

        //if (!dostringReselt)
        //{
        //    //7һ���ǵ��ֶ��ر������ 
        //    LuaVMAPI.lua_close(L);
        //    return 0;
        //}
        LuaVMAPI.DoString(L, luafile);
        //4��������ջ�ף�������̣���Ϊ��ȷ��ջ���ǿյģ��Ա����Ĳ����ǰ���˳����ջ���Ҵ�1��ջλ��ʼ
        LuaVMAPI.lua_settop(L, 0);


        // get global variable
        LuaVMAPI.lua_getglobal(L, "version");
      
        string version = LuaVMAPI.lua_tostring(L, -1); //��ջ����ȡ
        Debug.Log("version is "+ version);
       // return 0;

        //5��ȡ���ؽ�����ⲽ��ʼ C++ȥ�����������ջ���� ��LuaFunc"��ջ
        LuaVMAPI.lua_getglobal(L, "luaFunc");


        //1>  C++����Lua�������L)���ѽ�������LuaFunc"������ջ��ջ��һ��Ԫ�أ���LuaFunc" ��
        // ����2: "��������0��������
        //����3: �᷵��4��������
        // ����4: ����Ҫ������Ϣ��0��
        //2> ���C++���������ԣ���������ˣ��������L����ʼ����ջ����ջ��ȡ����LuaFunc"����ջ����Ԫ���ˣ�null��
        //3> ������õ� ��LuaFunc" ��Ϣ�͸� Lua���򣨱�����������ջ����Ԫ���ˣ�null��
        //4> Lua���� �ڵ��õ�Lua�ļ�ȫ�ֱ�Global table���в��� ��LuaFunc" �������з��ؽ����a,b,c,d������ջ����Ԫ���ˣ�null��
        //5> Lua����õ����ؽ����a,b,c,d�� �������ѹ��ջ��ѹ��˳��Ϊ��˳��ģ���a������ջ�ף���b������ջ���Դ����ơ���ջ���ĸ�Ԫ�أ�a,b,c,d����˳��Ϊջ��->ջ����
        //6>  ���C++���������ԣ���ȥջ�ж�ȡ���ݣ����� lua_tostring(L,1)�Ƕ�ȡ����������ı�ջ�ڵĽ���ģ����Ե��آ޲�ִ���꣬ջ�л����ĸ�Ԫ�أ�a,b,c,d
        //��ʾ����ʹ��lua_pop��L,1�� ȥ�����Ļ������Ե���ָ����λ�õ�ջ����

        //6����ջ���ؽ��           
        LuaVMAPI.lua_pcallk(L, 0, 4, 0);

        //��ӡջ���Ԫ��
        Debug.Log( LuaVMAPI.lua_tostring(L, 1));
        Debug.Log(LuaVMAPI.lua_tostring(L, 2));
        Debug.Log(LuaVMAPI.lua_tostring(L, 3));
        Debug.Log(LuaVMAPI.lua_tostring(L, 4));
        Debug.Log(LuaVMAPI.lua_tostring(L, 5));
        //7һ���ǵ��ֶ��ر������ 
        LuaVMAPI.lua_close(L);
    }

}
