using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalArray
{
    public interface IMarshalSafeArray
    {
        // 值传递
        int IntArrayUpdateByVal(int[] array);
        int IntArrayUpdateByValInOut([In, Out]int[] array);

        // 引用传递
        int IntArrayUpdateByRef(ref int[] array);
        int IntArrayUpdateByRefInOnly([In] ref int[] array);
        int[] IntArrayReturn(int elementNum);
    }

    [ClassInterface(ClassInterfaceType.None)]
    public class MarshalSafeArrayObj : IMarshalSafeArray
    {

        #region IMarshalSafeArray Members

        public int IntArrayUpdateByVal(int[] array)
        {
            return UpdateArray(array);
        }

        public int IntArrayUpdateByValInOut(int[] array)
        {
            return UpdateArray(array);
        }

        public int IntArrayUpdateByRef(ref int[] array)
        {
            return UpdateArray(array);
        }

        public int IntArrayUpdateByRefInOnly(ref int[] array)
        {
            return UpdateArray(array);
        }

        public int[] IntArrayReturn(int elementNum)
        {
            int[] newArray = new int[elementNum];
            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = i * 10;
            }
            return newArray;
        }

        private int UpdateArray(int[] array)
        {
            int result = 0;
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i];
                array[i] += 100;
            }
            return result;
        }

        #endregion
    }
}
