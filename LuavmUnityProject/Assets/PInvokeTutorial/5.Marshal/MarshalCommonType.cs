using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalCommonType
{

    public interface IMarshalCommonType
    {
        void MarshalInteger(int value);
        void MarshalChar(char value);
        void MarshalString(string value);
        void MarshalDate(DateTime value);
        void MarshalDecimal(decimal value);
        void MarshalCurrecny([MarshalAs(UnmanagedType.Currency)] decimal value);
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class MarshalCommonTypeObj : IMarshalCommonType
    {

        #region IMarshalCommonType Members

        public void MarshalInteger(int value)
        {
            UnityEngine.Debug.LogFormat(".NET中的类型：{0}, 值为：{1}", value.GetType().Name, value);
        }

        public void MarshalChar(char value)
        {
            UnityEngine.Debug.LogFormat(".NET中的类型：{0}, 值为：{1}", value.GetType().Name, value);
        }

        public void MarshalString(string value)
        {
            UnityEngine.Debug.LogFormat(".NET中的类型：{0}, 值为：{1}", value.GetType().Name, value);
        }

        public void MarshalDate(DateTime value)
        {
            UnityEngine.Debug.LogFormat(".NET中的类型：{0}, 值为：{1}", value.GetType().Name, value);
        }

        public void MarshalDecimal(decimal value)
        {
            UnityEngine.Debug.LogFormat(".NET中的类型：{0}, 值为：{1}", value.GetType().Name, value);
        }

        public void MarshalCurrecny(decimal value)
        {
            UnityEngine.Debug.LogFormat(".NET中的类型：{0}, 值为：{1}", value.GetType().Name, value);
        }

        #endregion
    }
}
