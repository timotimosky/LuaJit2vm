#include "WraperToUnity.h"

extern "C"
{

	//C ++端创建一个new GameObject，获取它transform，并每一帧设置它的position，直到它创建了10个。
	//由于C ++方面没有对C＃端定义的结构的可见性Vector3，因此需要在C ++中定义它们。

	// C# struct types
	struct Vector3
	{
		float x;
		float y;
		float z;
	};


	// 非托管回调函数定义
	//typedef int(__cdecl* PCallbackFunc)(int num0, int num1);

	// C# functions for C++ to call
	int (*GameObjectNew)();
	int (*GameObjectGetTransform)(int thisHandle);
	void (*TransformSetPosition)(int thisHandle, Vector3 position);

	// C++ functions for C# to call
	int numCreated;

	_DLLExport void Init(
		int (*gameObjectNew)(),
		int (*gameObjectGetTransform)(int),
		void (*transformSetPosition)(int, Vector3))
	{
		GameObjectNew = gameObjectNew;
		GameObjectGetTransform = gameObjectGetTransform;
		TransformSetPosition = transformSetPosition;

		numCreated = 0;
	}

	_DLLExport void MonoBehaviourUpdate(int thisHandle)
	{
		if (numCreated < 10)
		{
			int goHandle = GameObjectNew();
			int transformHandle = GameObjectGetTransform(goHandle);
			float comp = (float)numCreated;
			Vector3 position = { comp, comp, comp };
			TransformSetPosition(transformHandle, position);
			numCreated++;
		}
	}
}