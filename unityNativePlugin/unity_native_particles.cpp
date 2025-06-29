#include "renderer_opengl.h"
#include "globals.h"

#include <windows.h>
#include "../Unity/IUnityInterface.h" 
#include "../Unity/IUnityGraphics.h"
#include <cstdio>
#include <cstdint>

#define FP(...) extern "C" __VA_ARGS__ UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API

static void UNITY_INTERFACE_API on_graphics_device_event(UnityGfxDeviceEventType event_type);

Renderer* renderer = nullptr;
int particle_qty = 2'000'000;
bool disable_flx = false;
float _time = 0.f;
float _delta_time = 0.f;

FILE* stream;

struct UnityPointers {
	IUnityInterfaces* interfaces = nullptr;
	IUnityGraphics* graphics = nullptr;
	UnityGfxRenderer renderer_type = kUnityGfxRendererNull;
} unity;

/* Init */
FP(void) UnityPluginLoad(IUnityInterfaces* unity_interfaces) {
	unity.interfaces = unity_interfaces;

	unity.graphics = unity.interfaces->Get<IUnityGraphics>();
	unity.graphics->RegisterDeviceEventCallback(on_graphics_device_event);

	AllocConsole();
	SetConsoleTitle(L"Unity Native Particles");
	freopen_s(&stream, "CONOUT$", "w", stdout);

	if (!(renderer = new Renderer(static_cast<GLsizei>(particle_qty)))) {
		OUTPUT_ERROR("Problem constructing renderer. FlexiParts disabled until reload.\n");
		disable_flx = true;
	}

	/**
	 * Run OnGraphicsDeviceEvent(initialize) manually on plugin load
	 * to not miss the event in case the graphics device is already initialized
	 */
	on_graphics_device_event(kUnityGfxDeviceEventInitialize);
}

/* Destroy */
FP(void) UnityPluginUnload() {
	if (renderer) {
		delete renderer;
		renderer = nullptr;
	}

	FreeConsole();

	unity.graphics->UnregisterDeviceEventCallback(on_graphics_device_event);
}

#if UNITY_WEBGL // UNTESTED
typedef void	(UNITY_INTERFACE_API * PluginLoadFunc)(IUnityInterfaces* unityInterfaces);
typedef void	(UNITY_INTERFACE_API * PluginUnloadFunc)();
extern "C" void	UnityRegisterRenderingPlugin(PluginLoadFunc loadPlugin, PluginUnloadFunc unloadPlugin);

FP(void) RegisterPlugin() {
	UnityRegisterRenderingPlugin(unity_plugin_load, unity_plugin_unload);
}
#endif

static void UNITY_INTERFACE_API on_graphics_device_event(UnityGfxDeviceEventType event_type) {
	switch (event_type) {
		case kUnityGfxDeviceEventInitialize:
		{
			unity.renderer_type = unity.graphics->GetRenderer();

			if (unity.renderer_type != kUnityGfxRendererOpenGLCore) {
				OUTPUT_ERROR("Prototype only works on OpenGL. FlexiParts disabled until reload.\n");
				disable_flx = true;
				return;
			}

			if (renderer)
				renderer->init_opengl();

		} break;
		case kUnityGfxDeviceEventShutdown:
		{
			unity.renderer_type = kUnityGfxRendererNull;

			if (renderer)
				renderer->detroy_opengl();

		} break;
		case kUnityGfxDeviceEventBeforeReset:
		{
		} break;
		case kUnityGfxDeviceEventAfterReset:
		{
		} break;
	};
}

static void UNITY_INTERFACE_API flx_render(int eventId) {
	if (disable_flx)
		return;

	renderer->render(_time, _delta_time);
}

extern "C" UnityRenderingEvent UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API flx_get_render_event_func() {
	return flx_render;
}

FP(void) flx_update(int eventId, float time, float delta_time) {
	if (disable_flx)
		return;

	_time = time;
	_delta_time = delta_time;
	renderer->update(_time, _delta_time);
};

FP(void) flx_init(int32_t particle_qty) {
	if (renderer != nullptr) {
		OUTPUT_ERROR("Changing particle quantity after initialization is not yet supported.\n");
	}

}

FP(void) flx_set_mvp(GLfloat model[16], GLfloat view[16], GLfloat projection[16]) {
	if (renderer == nullptr)
		return;

	renderer->mvp(model, view, projection);
}

