using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalCommonType
{
    public interface IMarshalDirection
    {
        void IntegerDefault(int inArg, int outArg);
        void IntegerInOut(int inArg, [In, Out] int outArg);
        void IntegerRef(int inArg, ref int outArg);
        void IntegerRefIn(int inArg, [In] ref int outArg);
        void IntegerOut(int inArg, out int outArg);
        void StringDefault(string inArg, string outArg);
        void StringInOut(string inArg, [In, Out] string outArg);
        void StringRef(string inArg, ref string outArg);
        void StringRefIn(string inArg, [In] ref string outArg);
        void StringOut(string inArg, out string outArg);
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class MarshalDirectionObj : IMarshalDirection
    {

        #region IMarshalDirection Members

        public void IntegerDefault(int inArg, int outArg)
        {
            outArg = inArg;
        }

        public void IntegerInOut(int inArg, int outArg)
        {
            outArg = inArg;
        }

        public void IntegerRef(int inArg, ref int outArg)
        {
            outArg = inArg;
        }

        public void IntegerRefIn(int inArg, ref int outArg)
        {
            outArg = inArg;
        }

        public void IntegerOut(int inArg, out int outArg)
        {
            outArg = inArg;
        }

        public void StringDefault(string inArg, string outArg)
        {
            outArg = inArg;
        }

        public void StringInOut(string inArg, string outArg)
        {
            outArg = inArg;
        }

        public void StringRef(string inArg, ref string outArg)
        {
            outArg = inArg;
        }

        public void StringRefIn(string inArg, ref string outArg)
        {
            outArg = inArg;
        }

        public void StringOut(string inArg, out string outArg)
        {
            outArg = inArg;
        }

        #endregion
    }
}
