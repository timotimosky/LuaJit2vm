using Python.Runtime.Native;

using UnityEngine;

namespace Python.Runtime
{
    [System.Serializable]
    // BonusGamesHack: Dispatcher的补丁
    public partial class Dispatcher
    {
        /// <summary>
        /// 解除引用环
        /// </summary>
        public void BreakCycle()
        {
            // 判断对象是否销毁了
            if (this.target.IsDisposed)
            {
                Debug.LogError("[Python.Runtime.Dispatcher] Python对象已经被销毁 !!!");
                return;
            }
            
            this.target.Dispose();

            // TODO:Mark 之前py2的处理方法
            /*
            PyGILState state = PythonEngine.AcquireLock();
            Runtime.XDecref(this.target.Steal());
            PythonEngine.ReleaseLock(state);
            this.target = PyObject.None;
            */
        }

        public void Dispose()
        {
            this.BreakCycle();
        }
    }
}
