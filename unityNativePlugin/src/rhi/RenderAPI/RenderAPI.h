#pragma once
#include <stddef.h>
#include "../Unity/IUnityGraphics.h"
struct IUnityInterfaces;


// Super-simple "graphics abstraction". This is nothing like how a proper platform abstraction layer would look like;
// all this does is a base interface for whatever our plugin sample needs. Which is only "draw some triangles"
// and "modify a texture" at this point.
//
// There are implementations of this base class for D3D9, D3D11, OpenGL etc.; see individual RenderAPI_* files.

//�����򵥵ġ�ͼ�γ��󡱡�����һ�����ʵ�ƽ̨�������ȫ��ͬ;
//������Щ����һ�������Ľӿڣ�ֻ��Ϊ���������ǵĲ�����ӵ���Ҫ��Ҳ����"��һЩ������"�͡��޸������������ϡ�
//
//��D3D9, D3D11, OpenGL��������������ʵ��;�μ�������RenderAPI_*�ļ���
class RenderAPI
{
public:
	virtual ~RenderAPI() { }


	// Process general event like initialization, shutdown, device loss/reset etc.
	//����һ���¼������ʼ�����ػ����豸��ʧ/���õȡ�
	virtual void ProcessDeviceEvent(UnityGfxDeviceEventType type, IUnityInterfaces* interfaces) = 0;

	// Is the API using "reversed" (1.0 at near plane, 0.0 at far plane) depth buffer?
	// Reversed Z is used on modern platforms, and improves depth buffer precision.
	// API�Ƿ�ʹ����"reversed"(1.0�ڽ�ƽ�棬0.0��Զƽ��)��Ȼ�����?
	//���ִ�ƽ̨��ʹ�÷�תZ���������Ȼ������ľ��ȡ�
	virtual bool GetUsesReverseZ() = 0;

	// Draw some triangle geometry, using some simple rendering state.
	// Upon call into our plug-in the render state can be almost completely arbitrary depending on what was rendered in Unity before.
	//  Here, we turn off culling, blending, depth writes etc.
	// and draw the triangles with a given world matrix. The triangle data is
	// float3 (position) and byte4 (color) per vertex.

	//����һЩ�����μ��Σ�ʹ��һЩ�򵥵���Ⱦ״̬��
	//�ڵ������ǵĲ��ʱ����Ⱦ״̬����������ȫ���⣬��ȡ����֮ǰ��Unity����Ⱦ�����ݡ�
	//��������ǹر�ѡ�񣬻�ϣ����д��ȡ��ø��������������������Ρ�
	//����������Ϊ float3 (position)��byte4 (color)ÿ�����㡣
	virtual void DrawSimpleTriangles(const float worldMatrix[16], int triangleCount, const void* verticesFloat3Byte4) = 0;


	// Begin modifying texture data. You need to pass texture width/height too, since some graphics APIs
	// (e.g. OpenGL ES) do not have a good way to query that from the texture itself...
	//
	// Returns pointer into the data buffer to write into (or NULL on failure), and pitch in bytes of a single texture row.

	//��ʼ�޸��������ݡ���Ҳ��Ҫ��������Ŀ��/�߶ȣ���ΪһЩͼ��api(����OpenGL ES)û��һ���õķ�������ѯ��������
	//
	//����ָ�뵽���ݻ�����д��(��NULLʧ��)����Ͷ�뵥�������е��ֽڡ�
	virtual void* BeginModifyTexture(void* textureHandle, int textureWidth, int textureHeight, int* outRowPitch) = 0;
	// End modifying texture data.
	virtual void EndModifyTexture(void* textureHandle, int textureWidth, int textureHeight, int rowPitch, void* dataPtr) = 0;


	// Begin modifying vertex buffer data.
	// Returns pointer into the data buffer to write into (or NULL on failure), and buffer size.
	//��ʼ�޸Ķ��㻺�������ݡ�
	//����Ҫд�����ݻ�������ָ��(ʧ��ʱΪNULL)���Լ���������С��
	virtual void* BeginModifyVertexBuffer(void* bufferHandle, size_t* outBufferSize) = 0;
	// End modifying vertex buffer data.
	virtual void EndModifyVertexBuffer(void* bufferHandle) = 0;
};


// Create a graphics API implementation instance for the given API type.
RenderAPI* CreateRenderAPI(UnityGfxRenderer apiType);

