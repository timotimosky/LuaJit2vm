using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalOptionalParam
{
    public interface IMarshalOptional
    {
        int AddIntegerOptional(int arg1,
            [Optional, DefaultParameterValue(10)]
			int arg2);

        int AddVariantOptional(int arg1,
            [Optional]
            object arg2);
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class MarshalOptionalObj : IMarshalOptional
    {
        #region IMarshalOptional Members

        public int AddIntegerOptional(int arg1, int arg2)
        {
            return arg1 + arg2;
        }

        public int AddVariantOptional(int arg1, object arg2)
        {
            int intArg2;
            if (arg2 is System.Reflection.Missing)
            {
                // 默认值为10
                intArg2 = 10;
            }
            else if (arg2 is int)
            {
                intArg2 = (int)arg2;
            }
            else
            {
                throw new NotSupportOptionalParameterType("你传递的可选参数格式错误");
            }

            return arg1 + intArg2;
        }

        #endregion
    }

    public class NotSupportOptionalParameterType : ApplicationException
    {
        public NotSupportOptionalParameterType(string msg)
            : base(msg)
        {
            this.HResult = unchecked((int)0x80040500);
        }
    }
}
