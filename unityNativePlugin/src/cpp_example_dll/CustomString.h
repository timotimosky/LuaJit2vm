#pragma once
#include <string>
#include <string.h>
#include <stddef.h> 

//c++实现委托，有好几种形式 
//1. 定义函数指针
//2.定义函数，但用另一个函数封装，返回之前函数的指针
// callback definition for getlength and append function

//typedef void(__cdecl *text_callback)(bool hastextchanged, int newlength);
typedef void( *text_callback)(bool hastextchanged, int newlength);
namespace Cpp2UnityPInvoke {
	class CustomString {
	private:

	
	public:

		std::string mText;
		text_callback mCallback;
		CustomString();
		CustomString(const std::string&);
		void GetString(char* buf, size_t length);
		size_t GetLength();
		void Append(const std::string&);
		void SetCallback(text_callback callback);
	};
}