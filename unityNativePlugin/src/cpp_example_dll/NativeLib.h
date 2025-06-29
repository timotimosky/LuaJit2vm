// 下面的#ifdef代码段使用宏定义的方式，简化了导出动态链接库函数的声明。该动态链接库中所有
// 的文件都会按照编译器选项中指定的NATIVELIB_EXPORTS符号进行编译。
#ifdef NATIVELIB_EXPORTS
#define NATIVELIB_API __declspec(dllexport)
#else
#define NATIVELIB_API __declspec(dllimport)
#endif

// 示例4.1
// Managed C++中的平台调用和C++ Interop
extern "C" NATIVELIB_API int __cdecl MultiplyTwoNumbers(int numOne, int numTwo);
extern "C" NATIVELIB_API bool __cdecl ReverseString(const wchar_t* rawString, wchar_t* stringBuffer, int bufferSize);

// 示例4.2
class NATIVELIB_API CUnmanagedClass
{
public:
	CUnmanagedClass();
	virtual ~CUnmanagedClass();
	void PassInt(int intValue);
	void PassString(char* strValue);
	char* ReturnString();
};

// 示例4.6
// 导致运行时异常
extern "C" NATIVELIB_API void __cdecl CauseUnmanagedExceptions(int type);
// 用throw关键字显式抛出异常
extern "C" NATIVELIB_API void __cdecl ThrowUnmanagedExceptions(int type);

// 自定义C++异常类，被ThrowUnmanagedExceptions使用
class CUnmanagedException
{
public:
	CUnmanagedException(void) {}
	~CUnmanagedException(void)
	{
	    delete [] m_Message;
	}

	CUnmanagedException(const wchar_t* message, int errorCode)
	{
	    m_ErrorCode = errorCode;
		size_t bufLength = wcslen(message) + 1;
		m_Message   = new wchar_t[bufLength];
		wcscpy_s(m_Message, bufLength, message);
	}

	const wchar_t* GetErrorMessage()
	{
		return (const wchar_t*)m_Message;
	}

	int GetErrorCode()
	{
		return m_ErrorCode;
	}

private:
	wchar_t* m_Message;
	int   m_ErrorCode;
};