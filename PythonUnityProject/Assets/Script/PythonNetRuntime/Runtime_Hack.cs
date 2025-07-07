using System;
using UnityEngine;

namespace Python.Runtime
{
	public unsafe partial class Runtime
	{
		// BonusGamesHack: 这里需要根据操作系统宏来定义dll的静态常量
#if UNITY_STANDALONE_WIN
		public const string PYTHON_DLL = "python312";
#elif UNITY_ANDROID
		public const string PYTHON_DLL = "python3.12";
#elif UNITY_IOS
		// iOS只支持通过静态库编译到app内，所以dll的名称为"__Internal"
		public const string PYTHON_DLL = "__Internal";
#endif
		
		/// <summary>
		/// 检查是不是bytes类型
		/// </summary>
		/// <param name="ob"></param>
		/// <returns></returns>
		internal static bool PyBytes_Check(BorrowedReference ob) => PyObject_TypeCheck(ob, PyBytesType);
		
		/// <summary>
		/// 检查是不是bytearray类型
		/// </summary>
		/// <param name="ob"></param>
		/// <returns></returns>
		internal static bool PyByteArray_Check(BorrowedReference ob) => PyObject_TypeCheck(ob, PyByteArrayType);
		
		/// <summary>
		/// 获取bytearray类型的头指针
		/// </summary>
		/// <param name="ob"></param>
		/// <returns></returns>
		internal static IntPtr PyByteArray_AsString(BorrowedReference ob)
		{
			Debug.Assert(ob != null);
			return Delegates.PyByteArray_AsString(ob);
		}
	}
}
