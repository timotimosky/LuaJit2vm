#include "CustomString.h"
namespace Cpp2UnityPInvoke {

	/**
	 * default constructor
	 */

	CustomString::CustomString()
	{
		CustomString(std::string(""));
	}

	/**
	 * constructor takes a given string parameter
	 */
	CustomString::CustomString(const std::string& inputText) {
		mText = inputText;
	}

	/**
	 * retreive the string and put in into a given buffer
	 */
	void CustomString::GetString(char* buf, size_t length) {
		strncpy_s(buf, length, mText.c_str(), mText.size());
	}

	/**
	 * retreive the length of the string
	 */
	size_t CustomString::GetLength() {
		if (nullptr != mCallback) {
			mCallback(false, mText.size());
		}
		return mText.size();
	}

	/**
	 * append to the string
	 */
	void CustomString::Append(const std::string& str) {
		mText.append(str);
		if (nullptr != mCallback) {
			mCallback(str.size() > 0, mText.size());
		}
	}

	/**
	 * set callback to listen for the GetLength and Append call. This
	 * is for demostrating how to use callback from unity
	 */

	void CustomString::SetCallback(text_callback callback) {
		mCallback = callback;
	}

	/**
	* DLL exported function
	* set callback for listening GetLength and Append for an instance
	*/
	void SetCallback(CustomString* targetObj, text_callback callback) {
		targetObj->SetCallback(callback);
	}


	/**
	 * DLL exported function
	 * create a new instance of the class and return it
	 */
	CustomString* GetCustomString(const char* str, size_t strLength) {
		return new CustomString(std::string(str, strLength));
	}

	/**
	 * DLL exported function
	 * destroy the created instance and release memory
	 */
	void DestroyString(CustomString* targetObj) {
		delete targetObj;
	}

	/**
	 * DLL exported function
	 * retreive the string of an instance
	 * 这里做一个指针的映射
	 */
	void GetString(CustomString* targetObj, char* buf, size_t length) {
		targetObj->GetString(buf, length);
	}

	/**
	* DLL exported function
	* retreive the length of the string for an instance
	*/
	size_t GetLength(CustomString* targetObj) {
		return targetObj->GetLength();
	}

	/**
	* DLL exported function
	* append string for an instance
	*/
	void Append(CustomString* targetObj, const char* text, size_t textLength) {
		return targetObj->Append(std::string(text, textLength));
	}


}