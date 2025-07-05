using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace MarshalFullExample
{

    [Flags]
    public enum NameEntityType
    {
        PersonName,
        PlaceName,
        OrganizationName
    }

    //typedef struct _NAMEENTITY
    //{
    //    wchar_t* _name;
    //    NameEntityType _type;
    //    int _highlight_begin;
    //    int _highlight_length;
    //    double _score;
    //}NAMEENTITY, *PNAMEENTITY;
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class NameEntity
    {
        private string _name;
        private NameEntityType _type;
        private int _highlightBegin;
        private int _highlightLength;
        private double _score;

        #region Properties
        public string Name
        {
            get { return _name; }
        }

        public NameEntityType Type
        {
            get { return _type; }
        }

        public int HighlightBegin
        {
            get { return _highlightBegin; }
        }

        public int HighlightLength
        {
            get { return _highlightLength; }
        }

        public double Score
        {
            get { return _score; }
        }
        #endregion
    }

    class NameFinderWrapper : IDisposable
    {
        // 创建对象，并返回一个句柄
        // NATIVELIB_API NAMEFINDERHANDLE CreateNameFinderInstance(__in NameEntityType type);
        [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static NameFinderSafeHandle CreateNameFinderInstance(NameEntityType type);

        // 初始化对象，比如一些模型和运行时依赖的数据
        // NATIVELIB_API bool Initialize(__in NAMEFINDERHANDLE hHandle, __in_z const wchar_t* resourcePath);
        [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static bool Initialize(NameFinderSafeHandle hHandle, string resourcePath);

        // 根据给定的一段文字，返回其中包含的各种名字
        // NATIVELIB_API bool FindNames(
        //    __in NAMEFINDERHANDLE hHandle,
        //    __in_z const wchar_t* text, 
        //    __out PNAMEENTITY* nameArray,
        //    __out UINT* arraySize);
        [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        private extern static bool FindNames(
            NameFinderSafeHandle hHandle,
            string text,
            out IntPtr nameArray,
            out uint arraySize);

        // 释放对象资源
        // NATIVELIB_API void UnInitialize(__in NAMEFINDERHANDLE hHandle);
        [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static void UnInitialize(IntPtr hHandle);

        // 销毁对象
        // NATIVELIB_API void DestroyInstance(__in NAMEFINDERHANDLE hHandle);
        [DllImport(DllLoader._dllName, CallingConvention = CallingConvention.Cdecl)]
        private extern static void DestroyInstance(IntPtr hHandle);

        /// <summary>
        /// Override ReleaseHandle method to release resource
        /// </summary>
        private class NameFinderSafeHandle : SafeNativeHandle
        {
            protected override bool ReleaseHandle()
            {
                NameFinderWrapper.UnInitialize(handle);
                NameFinderWrapper.DestroyInstance(handle);
                SetHandle(IntPtr.Zero);
                return true;
            }
        }

        private NameFinderSafeHandle _handle;

        public NameFinderWrapper(NameEntityType type)
        {
            _handle = CreateNameFinderInstance(type);
        }

        public bool Initialize(string resourcePath)
        {
            return Initialize(_handle, resourcePath);
        }

        public bool FindNames(string text, out List<NameEntity> nameList)
        {
            nameList = new List<NameEntity>();

            IntPtr arrayPtr = IntPtr.Zero;
            uint arraySize;
            if (_handle.IsInvalid)
            {
                return false;
            }

            if (FindNames(_handle, text, out arrayPtr, out arraySize))
            {
                NameEntity[] names = new NameEntity[arraySize];
                IntPtr cur = arrayPtr;
                for (int i = 0; i < arraySize; i++)
                {
                    names[i] = new NameEntity();
                    Marshal.PtrToStructure(cur, names[i]);
                    Marshal.DestroyStructure(cur, typeof(NameEntity));
                    cur = (IntPtr)((int)cur + Marshal.SizeOf(names[i]));
                }
                Marshal.FreeCoTaskMem(arrayPtr);
                nameList.AddRange(names);

                return true;
            }
            else
            {
                return false;
            }
        }

        // IDisposable接口的实现
        #region IDisposable Members

        public void Dispose()
        {
            _handle.Dispose();
        }

        #endregion
    }
}
