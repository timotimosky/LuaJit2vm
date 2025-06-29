#define _CRT_SECURE_NO_WARNINGS
#include "WraperToUnity.h"
#include "CustomString.h"
#include <iostream>
#include <stddef.h> 

_DLLExport float get_float()
{
	return 11.1f;
}

_DLLExport void get_float_ptr(float* value)
{
	*value = 52.1f;
}

_DLLExport void get_float_ref(float& value)
{
	value = 53.1f;
}

_DLLExport void get_string_value(char** str_ptr)
{
	const char* str = "hello world";
	strcpy(*str_ptr, str);
	return;
}

_DLLExport void set_string_value(char* s)
{
	std::string str = s;
	printf("str %s length %d", str.c_str(), str.length());
	return;
}

struct ANativeStruct
{
	int value1;
	float value2;
	int value3[10];
};


_DLLExport void SetStruct(ANativeStruct* stu, int count)
{
	stu->value1 = 12;
	stu->value2 = 12.1f;
	for (int i = 0; i < count; i++)
	{
		stu->value3[i] = i;
	}
	return ;
}

_DLLExport ANativeStruct* GetNewStruct()
{
	ANativeStruct* stu = new ANativeStruct();
	stu->value1 = 12;
	stu->value2 = 12.1f;
	for (int i = 0; i < 10; i++)
	{
		stu->value3[i] = i;
	}
	return stu;
}

/**
 * Library class declaration
 * 需要声明这个类
 */
class CustomString;

/**
 * Exported functions
 */
_DLLExport CustomString* GetCustomString(const char* str, size_t strLength);

_DLLExport void DestroyString(CustomString* targetObj);

_DLLExport void GetString(CustomString* targetObj, char* buf, size_t length);

_DLLExport size_t GetLength(CustomString* targetObj);

_DLLExport void Append(CustomString* targetObj, const char* str, size_t strLength);

//_DLLExport void SetCallback(CustomString* targetObj, TEXT_CALLBACK callback);


// Function pointer to the C# function
// The syntax is like this: ReturnType (*VariableName)(ParamType ParamName, ...)
int(*CsharpFunction)(int a, float b);

// C＃调用的C ++函数
	//获取C ++函数的函数指针
void Init(int(*csharpFunctionPtr)(int, float))
{
	CsharpFunction = csharpFunctionPtr;
}

// Example function that calls into C#
void Foo()
{
	// It's just a function call like normal!
	int retVal = CsharpFunction(2, 3.14f);
}