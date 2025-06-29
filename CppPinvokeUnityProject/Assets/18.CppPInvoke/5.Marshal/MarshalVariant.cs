using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalVariant
{
    public interface IMarshalVariant
    {
        // ��ͨVariant����
        string MarshalVariant(object inArg, ref object outArg);

        // ����Variant����
        // VT_CY
        string MarshalVariantCurrency(object inArg, ref object outArg);
        string MarshalVariantCurrencyWrapper(object inArg, ref object outArg);

        // VT_BSTR
        object MarshalVariantStringNull();
        object MarshalVariantStringNullWrapper();
        object MarshalVariantStringNonNullWrapper();

        // VT_DISPATCH �� VT_UNKNOWN
        void MarshalVariantClass(object inArg, ref object outArg);
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class MarshalVariantObj : IMarshalVariant
    {

        #region IMarshalVariant Members

        public string MarshalVariant(object inArg, ref object outArg)
        {
            outArg = inArg;
            return inArg.GetType().Name;
        }

        public string MarshalVariantCurrency(object inArg, ref object outArg)
        {
            outArg = inArg;
            return inArg.GetType().Name;
        }

        public string MarshalVariantCurrencyWrapper(object inArg, ref object outArg)
        {
            outArg = new CurrencyWrapper(inArg);
            return inArg.GetType().Name;
        }

        public object MarshalVariantStringNull()
        {
            return null;
        }

        public object MarshalVariantStringNullWrapper()
        {
            return new BStrWrapper(null);
        }

        public object MarshalVariantStringNonNullWrapper()
        {
            return new BStrWrapper("����BStrWrapper�ַ���");
        }

        public void MarshalVariantClass(object inArg, ref object outArg)
        {
            outArg = inArg;
            IDemoItemEx obj = outArg as IDemoItemEx;
            if (null != obj)
            {
                obj.IntValue = 2;
                obj.StringValue = "����Դ";
            }
        }

        #endregion
    }

    public interface IDemoItemEx
    {
        int IntValue { get; set; }
        string StringValue { get; set; }
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class DemoItemExObj : IDemoItemEx
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
