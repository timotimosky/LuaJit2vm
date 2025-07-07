using System;

namespace Python.Runtime
{
    // BonusGamesHack: PythonEngine补丁
    public partial class PythonEngine
    {
        // 支持注册委托
        public static void RegDelegate(Type dtype, Type stype)
        {
            delegateManager.RegDispatcher(dtype, stype);
        }
        
        // 导入模块
        public static PyObject ImportModule(string name)
        {
            NewReference op = Runtime.PyImport_ImportModule(name);
            if (op.IsNull())
            {
                if (Exceptions.ErrorOccurred())
                {
                    //throw new PythonException();
                }

                return null;
            }

            return op.MoveToPyObject();
        }
    }
}
