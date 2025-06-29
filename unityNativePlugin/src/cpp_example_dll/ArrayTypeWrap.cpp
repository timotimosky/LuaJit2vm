#define _CRT_SECURE_NO_WARNINGS
#include "WraperToUnity.h"
#include "CustomString.h"
#include <iostream>
#include <stddef.h> 

_DLLExport void get_int_arr1(int* arr, int count)
{
	for (int i = 0; i < count; i++)
	{
		arr[i] = i;
	}
	return;
}

_DLLExport void get_int_arr2(int* arr, int count)
{
	for (int i = 0; i < count; i++)
	{
		arr[i] = i;
	}
	return;
}
_DLLExport void get_int_arr3(int* arr, int count)
{
	for (int i = 0; i < count; i++)
	{
		arr[i] = i;
	}
	return;
}