using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalStructure
{
    public interface IMarshalClass
    {
        IDataClass CreateInstance();
        void UpdateInstance(IDataClass obj);
    }

    public interface IDataClass
    {
        int IntegerValue { get; }

        string StringValue 
        {
            get;
            
            [ComVisible(false)]
            set;
        }
    }

    [ComVisible(false)]
    public class DataClass : IDataClass
    {
        int _intValue;
        string _stringValue;

        #region IDataClass Members

        public int IntegerValue
        {
            get { return _intValue; }
            set { _intValue = value; }
        }

        public string StringValue
        {
            get { return _stringValue; }
            set { _stringValue = value; }
        }

        #endregion
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class MarshalClassObj : IMarshalClass
    {

        #region IMarshalClass Members

        public IDataClass CreateInstance()
        {
            DataClass obj = new DataClass();
            obj.IntegerValue = 10;
            obj.StringValue = "123";
            return obj;
        }

        public void UpdateInstance(IDataClass obj)
        {
            if (null != obj)
            {
                obj.StringValue = "Ò»¶þÈý";
            }
        }

        #endregion
    }

}
