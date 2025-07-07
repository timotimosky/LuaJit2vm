using Python.Runtime;
using Python.Runtime.Native;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class InvokePythonTest : MonoBehaviour
{

    void Start()
    {
        string CurrentPath = System.IO.Directory.GetCurrentDirectory();
        UnityEngine.Debug.Log(CurrentPath);
        string pycode_folder_path = CurrentPath + @"\Assets\script\pythonScript\PixetDemo.py";//存放python脚本的目录

        string pycode_folder_path2 = CurrentPath + @"\Assets\script\pythonScript";
        //使用流的形式读取
        string sr = FileHelp.ReadFileText(pycode_folder_path);

        byte[] dateArray = System.Text.Encoding.ASCII.GetBytes(sr);

        FileHelp.WriteModelFile(pycode_folder_path2, "PixetDemo2.py", dateArray);

        MainTest();
   
    }

    private void TestImportModule(string module_name, string[] attrs)
    {
        StrPtr module_name_ptr = new StrPtr(module_name, Encoding.UTF8);
        NewReference module_reference = Runtime.Delegates.PyImport_ImportModule(module_name_ptr);
        if (module_reference.IsNull())
        {
           UnityEngine.Debug.LogError($"====== [{module_name}] is null ======");
            return;
        }

        Debug.Log($"====== [{module_name}] exist ======");

        foreach (string attr in attrs)
        {
            StrPtr attr_ptr = new StrPtr(attr, Encoding.UTF8);
            NewReference n = Runtime.Delegates.PyObject_GetAttrString(module_reference.Borrow(), attr_ptr);
            if (n.IsNull())
            {
                continue;
            }
            string show = n.IsNull() ? "null" : "exist";
            Debug.Log($"[{module_name}].[{attr}] ptr [{show}]");
        }
    }

    public void MainTest()
    {
        PythonManager.Init();

        // 测试模块导入
        this.TestImportModule("os", new string[] { "name", "environ" });
        this.TestImportModule("sys", new string[] { "platform", "version", "path" });
        this.TestImportModule("builtins", new string[] { "object", "len", "NotImplemented", "None", "True", "False", "super" });
        this.TestImportModule("math", new string[] { "pi" });
        this.TestImportModule("importlib.abc", new string[] { "Loader", "MetaPathFinder" });
        this.TestImportModule("_ctypes", new string[] { "__version__" });


        Debug.Log("初始化python虚拟机成功");

         PythonManager.CallPythonModuleFunction("Download", "download_call_by_c");



        using (Py.GIL()) //Initialize the Python engine and acquire the interpreter lock
        {
           // PythonManager.CallPythonModuleFunction("Download", "download_call_by_c");

            using var scope = Py.CreateScope();
            scope.Exec("print('a b c')");

           // dynamic np = Py.Import("numpy");
           // Console.WriteLine(np.cos(np.pi * 2));
            //dynamic sin = np.sin;
           // Console.WriteLine(sin(5));

            PythonEngine.RunSimpleString("print('a b c')");

            //try

            {
               // dynamic np = Py.Import("numpy");
               //  Debug.Log(np.cos(np.pi * 2));


                // import your script into the process
                dynamic sampleModule = Py.Import("PixetDemo");//python脚本文件名
                int x = 3;
                int y = 4;
                dynamic results = sampleModule.PixetInit(x, y);
                Debug.Log("Results: " + results);
            }
            //catch (PythonException error)
            //{
            //    // Communicate errors with exceptions from within python script -
            //    // this works very nice with pythonnet.
            //    Debug.Log("Error occured: "+ error.Message);
            //}


        }

    }

    private void OnDestroy()
    {
        // 在销毁时关闭Python引擎
        PythonEngine.Shutdown();
    }
}
