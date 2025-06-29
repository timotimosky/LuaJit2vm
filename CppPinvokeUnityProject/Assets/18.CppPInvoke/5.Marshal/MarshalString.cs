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
            outArg = inArg + "_�����(ref)";
        }

        public void StringOut(string inArg, out string outArg)
        {
            outArg = inArg + "_�����(out)";
        }

        public string StringReturn(string inArg)
        {
            return inArg + "_�����(return)";
        }

        public void StringStringBuilder(string inArg, StringBuilder outArg)
        {
            // ɾ��ԭ������
            outArg.Remove(0, outArg.Length);

            // ȷ��StringBuilder���������㹻������
            outArg.EnsureCapacity(inArg.Length + "_�����(StringBuilder)".Length);

            outArg.Append(inArg);
            outArg.Append("_�����(StringBuilder)");
        }

        public string StringCStyle(string inArg)
        {
            return inArg + "_�����(c-style)";
        }

        #endregion
    }
}
