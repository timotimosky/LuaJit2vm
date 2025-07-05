using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalString
{
    public interface IMarshalString
    {
        void StringRef(string inArg, ref string outArg);
        void StringOut(string inArg, out string outArg);
        string StringReturn(string inArg);
        void StringStringBuilder(string inArg, StringBuilder outArg);

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string StringCStyle([MarshalAs(UnmanagedType.LPWStr)] string inArg);
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class MarshalStringObj : IMarshalString
    {

        #region IMarshalString Members

        public void StringRef(string inArg, ref string outArg)
        {
            outArg = inArg + "_处理后(ref)";
        }

        public void StringOut(string inArg, out string outArg)
        {
            outArg = inArg + "_处理后(out)";
        }

        public string StringReturn(string inArg)
        {
            return inArg + "_处理后(return)";
        }

        public void StringStringBuilder(string inArg, StringBuilder outArg)
        {
            // 删除原有内容
            outArg.Remove(0, outArg.Length);

            // 确保StringBuilder对象中有足够的容量
            outArg.EnsureCapacity(inArg.Length + "_处理后(StringBuilder)".Length);

            outArg.Append(inArg);
            outArg.Append("_处理后(StringBuilder)");
        }

        public string StringCStyle(string inArg)
        {
            return inArg + "_处理后(c-style)";
        }

        #endregion
    }
}
