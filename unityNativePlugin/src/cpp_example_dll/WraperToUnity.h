#ifndef _TEST_DLL_H_  
#define _TEST_DLL_H_  
#endif  


// Unity native plugin API
// Compatible with C99

//只兼容C99。CPP直接在函数那里封装

#if defined(__CYGWIN32__)
	#define UNITY_INTERFACE_API __stdcall
	#define UNITY_INTERFACE_EXPORT __declspec(dllexport)
	#define UNITY_INTERFACE_IMPORT __declspec(dllimport)
#elif defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(_WIN64) || defined(WINAPI_FAMILY)
	#define UNITY_INTERFACE_API __stdcall
	#define UNITY_INTERFACE_EXPORT __declspec(dllexport)
	#define UNITY_INTERFACE_IMPORT __declspec(dllimport)
#elif defined(__MACH__) || defined(__ANDROID__) || defined(__linux__)
	#define UNITY_INTERFACE_API
	#define UNITY_INTERFACE_EXPORT
#else
	#define UNITY_INTERFACE_API
	#define UNITY_INTERFACE_EXPORT
#endif

#define EXPORTBUILD
#if defined (EXPORTBUILD)  
	# define _DLLExport extern "C"  UNITY_INTERFACE_EXPORT
# else  
	# define _DLLExport extern "C" UNITY_INTERFACE_IMPORT
#endif  
