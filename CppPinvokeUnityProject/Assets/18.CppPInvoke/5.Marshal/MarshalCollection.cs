using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalCollection
{
    public interface IMarshalCollection
    {
        int Add(object value);
        void Clear();
        bool Contains(object value);
        int IndexOf(object value);
        void Insert(int index, object value);
        void Remove(object value);
        void RemoveAt(int index);

        // DISPID_NEWENUM
        [DispId(-4)]
        System.Collections.IEnumerator GetEnumerator();

        // DISPID_VALUE
        [DispId(0)]
        object this[int index]
        {
            get;
        }
    }

    [ClassInterface(ClassInterfaceType.None)]
    [ComDefaultInterface(typeof(IMarshalCollection))]
    public class MarshalCollectionObj : System.Collections.ArrayList, IMarshalCollection
    {

        #region IMarshalCollection Members

        System.Collections.IEnumerator IMarshalCollection.GetEnumerator()
        {
            return base.GetEnumerator();
        }

        #endregion
    }

    public interface IDemoItem
    {
        int IntValue { get; set; }
        string StringValue { get; set; }
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class DemoItemObj : IDemoItem
    {
        int _intValue;
        string _stringValue;

        public int IntValue
        {
            get { return _intValue; }
            set { _intValue = value; }
        }

        public string StringValue
        {
            get { return _stringValue; }
            set { _stringValue = value; }
        }
    }

}
