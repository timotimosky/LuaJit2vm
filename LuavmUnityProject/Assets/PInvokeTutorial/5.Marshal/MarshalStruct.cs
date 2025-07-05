using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalStructure
{
    public interface IMarshalStruct
    {
        DataStruct CreateInstance();
        void UpdateInstance(ref DataStruct obj);
    }

    public struct DataStruct
    {
        public int IntegerValue;

        [MarshalAs(UnmanagedType.BStr)]
        public string StringValue;
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class MarshalStructObj : IMarshalStruct
    {

        #region IMarshalStruct Members

        public DataStruct CreateInstance()
        {
            DataStruct obj = new DataStruct();
            obj.IntegerValue = 10;
            obj.StringValue = "123";
            return obj;
        }

        public void UpdateInstance(ref DataStruct obj)
        {
            obj.StringValue = "Ò»¶þÈý";
        }

        #endregion
    }
}
