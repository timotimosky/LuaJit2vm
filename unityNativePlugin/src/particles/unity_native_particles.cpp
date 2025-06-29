#include <windows.h>
#include <cstdio>
#include <cstdint>
#include "../openglEx/renderer_opengl.h"
#include "../openglEx/globals.h"
#include "../rhi/Unity/IUnityInterface.h"
#include "../rhi/Unity/IUnityGraphics.h"


#define FP(...) extern "C" __VA_ARGS__ UNITY_INTERFACE_EXPORT UNITY_INTERFACE_API



Renderer* renderer = nullptr;
int particle_qty = 2'000'000;
bool disable_flx = false;
float _time = 0.f;
float _delta_time = 0.f;

FILE* stream;


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

