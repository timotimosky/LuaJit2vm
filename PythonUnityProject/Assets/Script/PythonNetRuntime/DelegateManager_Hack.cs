using System;

namespace Python.Runtime
{
    // BonusGames: 委托的补丁
    internal partial class DelegateManager
    {
        // 支持注册Dispatcher
        public void RegDispatcher(Type dtype, Type stype)
        {
            this.cache[dtype] = stype;
        }

        public bool IsPythonHandle(Delegate d)
        {
            return ((d != null) && (d.Target is Dispatcher));
        }

        public void TryDispose(Delegate d)
        {
            if (d == null)
            {
                return;
            }
            
            Dispatcher dispatcher = d.Target as Dispatcher;
            if (dispatcher == null)
            {
                return;
            }
            
            dispatcher.Dispose();
        }
    }
}
