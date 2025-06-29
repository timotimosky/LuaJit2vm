#pragma once
#include <stddef.h>
#include "../Unity/IUnityGraphics.h"
struct IUnityInterfaces;


// Super-simple "graphics abstraction". This is nothing like how a proper platform abstraction layer would look like;
// all this does is a base interface for whatever our plugin sample needs. Which is only "draw some triangles"
// and "modify a texture" at this point.
//
// There are implementations of this base class for D3D9, D3D11, OpenGL etc.; see individual RenderAPI_* files.

//超级简单的“图形抽象”。这与一个合适的平台抽象层完全不同;
//所有这些都是一个基本的接口，只是为了满足我们的插件例子的需要。也就是"画一些三角形"和“修改纹理”在像素上。
//
//在D3D9, D3D11, OpenGL等中有这个基类的实现;参见单独的RenderAPI_*文件。
class RenderAPI
{
public:
	virtual ~RenderAPI() { }


	// Process general event like initialization, shutdown, device loss/reset etc.
	//处理一般事件，如初始化，关机，设备丢失/重置等。
	virtual void ProcessDeviceEvent(UnityGfxDeviceEventType type, IUnityInterfaces* interfaces) = 0;

	// Is the API using "reversed" (1.0 at near plane, 0.0 at far plane) depth buffer?
	// Reversed Z is used on modern platforms, and improves depth buffer precision.
	// API是否使用了"reversed"(1.0在近平面，0.0在远平面)深度缓冲区?
	//在现代平台上使用反转Z，提高了深度缓冲区的精度。
	virtual bool GetUsesReverseZ() = 0;

	// Draw some triangle geometry, using some simple rendering state.
	// Upon call into our plug-in the render state can be almost completely arbitrary depending on what was rendered in Unity before.
	//  Here, we turn off culling, blending, depth writes etc.
	// and draw the triangles with a given world matrix. The triangle data is
	// float3 (position) and byte4 (color) per vertex.

	//绘制一些三角形几何，使用一些简单的渲染状态。
	//在调用我们的插件时，渲染状态几乎可以完全任意，这取决于之前在Unity中渲染的内容。
	//在这里，我们关闭选择，混合，深度写入等。用给定的世界矩阵绘制三角形。
	//三角形数据为 float3 (position)和byte4 (color)每个顶点。
	virtual void DrawSimpleTriangles(const float worldMatrix[16], int triangleCount, const void* verticesFloat3Byte4) = 0;


	// Begin modifying texture data. You need to pass texture width/height too, since some graphics APIs
	// (e.g. OpenGL ES) do not have a good way to query that from the texture itself...
	//
	// Returns pointer into the data buffer to write into (or NULL on failure), and pitch in bytes of a single texture row.

	//开始修改纹理数据。你也需要传递纹理的宽度/高度，因为一些图形api(例如OpenGL ES)没有一个好的方法来查询纹理本身……
	//
	//返回指针到数据缓冲区写入(或NULL失败)，并投入单个纹理行的字节。
	virtual void* BeginModifyTexture(void* textureHandle, int textureWidth, int textureHeight, int* outRowPitch) = 0;
	// End modifying texture data.
	virtual void EndModifyTexture(void* textureHandle, int textureWidth, int textureHeight, int rowPitch, void* dataPtr) = 0;


	// Begin modifying vertex buffer data.
	// Returns pointer into the data buffer to write into (or NULL on failure), and buffer size.
	//开始修改顶点缓冲区数据。
	//返回要写入数据缓冲区的指针(失败时为NULL)，以及缓冲区大小。
	virtual void* BeginModifyVertexBuffer(void* bufferHandle, size_t* outBufferSize) = 0;
	// End modifying vertex buffer data.
	virtual void EndModifyVertexBuffer(void* bufferHandle) = 0;
};


// Create a graphics API implementation instance for the given API type.
RenderAPI* CreateRenderAPI(UnityGfxRenderer apiType);

