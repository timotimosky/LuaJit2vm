// CppClient.cpp : ���ļ����� "main" ����������ִ�н��ڴ˴���ʼ��������
//

#include <iostream>
#include <windows.h>

int main()
{
	HMODULE hInst;
	hInst = LoadLibrary(TEXT("CppWrapper.dll"));
	if (hInst == NULL)
	{
		printf("����CppWrapper.dll��̬��ʧ��\n");
		return 0;
	}
	typedef int(*CallWrapper)();
	CallWrapper callWrapper = (CallWrapper)GetProcAddress(hInst, "CallWrapper");
	callWrapper();

	return 0;
}

// ���г���: Ctrl + F5 ����� >����ʼִ��(������)���˵�
// ���Գ���: F5 ����� >����ʼ���ԡ��˵�

// ����ʹ�ü���: 
//   1. ʹ�ý��������Դ�������������/�����ļ�
//   2. ʹ���Ŷ���Դ�������������ӵ�Դ�������
//   3. ʹ��������ڲ鿴���������������Ϣ
//   4. ʹ�ô����б��ڲ鿴����
//   5. ת������Ŀ��>���������Դ����µĴ����ļ�����ת������Ŀ��>�����������Խ����д����ļ���ӵ���Ŀ
//   6. ��������Ҫ�ٴδ򿪴���Ŀ����ת�����ļ���>���򿪡�>����Ŀ����ѡ�� .sln �ļ�
