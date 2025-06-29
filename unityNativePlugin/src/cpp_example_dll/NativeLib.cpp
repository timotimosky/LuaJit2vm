// NativeLib.cpp : Defines the entry point for the DLL application.
//

#include "stdafx.h"
#include "NativeLib.h"
#include <stdio.h>
#include <exception>

// 示例4.1
// Managed C++中的平台调用和C++ Interop
int __cdecl MultiplyTwoNumbers(int numOne, int numTwo)
{
	return numOne * numTwo;
}

bool __cdecl ReverseString(const wchar_t* rawString, wchar_t* stringBuffer, int bufferSize)
{
	int strLength = (int)wcslen(rawString);
	if(strLength >= bufferSize)
	{
		return false;
	}

	wcscpy_s(stringBuffer, bufferSize, rawString);

	_wcsrev(stringBuffer);
	
	return true;
}

// 示例4.2
CUnmanagedClass::CUnmanagedClass()
{
}

CUnmanagedClass::~CUnmanagedClass()
{
}

void CUnmanagedClass::PassInt(int intValue)
{
	printf("PassInt() called\n");
	printf("Interger value: %d\n", intValue);
}

void CUnmanagedClass::PassString(char* strValue)
{
	printf("PassString() called\n");
	printf("String value: %s\n", strValue);
}

char* CUnmanagedClass::ReturnString()
{
	printf("ReturnString() called\n");
	char* pStr = new char[128];
	strcpy_s(pStr, 128, "String from unmanaged code");
	return pStr;
}

// 示例4.6
void __cdecl CauseUnmanagedExceptions(int type)
{
	if(0 == type)
	{
		// 除以0
		int dividend = 0;
		int result = 3/dividend;
	}
	else if(1 == type)
	{
		// 操作非法内存
		char* pNULL = NULL;
		strcpy_s(pNULL, 128, "A String.");
		//strcpy(pNULL, "A String.");
	}
}

void __cdecl ThrowUnmanagedExceptions(int type)
{
	if(2 == type)
	{
		throw 123;
	}
	else if(3 == type)
	{
		throw CUnmanagedException(L"Unmanaged custom exception", 999);			
	}
	else if(4 == type)
	{
		throw std::exception("Unmanaged std exception");
	}
}