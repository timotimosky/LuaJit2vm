#pragma once
#include "glext.h"
#include <cstdio>

#define OUTPUT_ERROR(format, ...) printf("%s(%d) : %s() : " format "\n", __FILE__, __LINE__, __func__, ##__VA_ARGS__)
#define OUTPUT_MSG(format, ...) printf("" format "\n", ##__VA_ARGS__)

static char _err[64] = "";
#define GL_CHECK_ERROR() 	{ \
	gl_error_string(_err); \
	if (strlen(_err) != 0) { \
		OUTPUT_ERROR("OpenGL error : %s", _err); \
		memset(_err, 0, sizeof(_err)); \
	} \
}

const static char* gl_func_names[] = {
	"glCreateShaderProgramv"
	, "glGenProgramPipelines"
	, "glBindProgramPipeline"
	, "glUseProgramStages"
	, "glProgramUniform4fv"
	, "glGetProgramiv"
	, "glGetProgramInfoLog"
	, "glGenVertexArrays"
	, "glBindVertexArray"
	, "glGenBuffers"
	, "glBindBuffer"
	, "glBufferData"
	, "glVertexAttribPointer"
	, "glEnableVertexAttribArray"
	, "glUseProgram"
	, "glDisableVertexAttribArray"
	, "glDeleteVertexArrays"
	, "glBindFragDataLocation"
	, "glActiveShaderProgram"
	, "glGetUniformLocation"
	, "glProgramUniformMatrix4fv"
	, "glProgramUniformMatrix3fv"
	, "glCreateProgram"
	, "glCreateShader"
	, "glShaderSource"
	, "glCompileShader"
	, "glAttachShader"
	, "glLinkProgram"
	, "glBindAttribLocation"
	, "glGetShaderiv"
	, "glGetShaderInfoLog"
	, "glUniformMatrix4fv"
	, "glUniformMatrix3fv"
	, "glDeleteBuffers"
	, "glDetachShader"
	, "glDeleteShader"
	, "glDeleteProgram"
	, "glUniform1f"
	, "glBufferSubData"
	, "glVertexAttribDivisor"
	, "glDrawArraysInstanced"
	, "glIsVertexArray"
};

static const size_t gl_num_funcs = sizeof(gl_func_names) / sizeof(char*);
static void* gl_funcs[gl_num_funcs];
#define oglCreateShaderProgramv		((PFNGLCREATESHADERPROGRAMVPROC)gl_funcs[0])
#define oglGenProgramPipelines		((PFNGLGENPROGRAMPIPELINESPROC)gl_funcs[1])
#define oglBindProgramPipeline		((PFNGLBINDPROGRAMPIPELINEPROC)gl_funcs[2])
#define oglUseProgramStages			((PFNGLUSEPROGRAMSTAGESPROC)gl_funcs[3])
#define oglProgramUniform4fv		((PFNGLPROGRAMUNIFORM4FVPROC)gl_funcs[4])
#define oglGetProgramiv				((PFNGLGETPROGRAMIVPROC)gl_funcs[5])
#define oglGetProgramInfoLog		((PFNGLGETPROGRAMINFOLOGPROC)gl_funcs[6])
#define oglGenVertexArrays			((PFNGLGENVERTEXARRAYSPROC)gl_funcs[7])
#define oglBindVertexArray			((PFNGLBINDVERTEXARRAYPROC)gl_funcs[8])
#define oglGenBuffers				((PFNGLGENBUFFERSPROC)gl_funcs[9])
#define oglBindBuffer				((PFNGLBINDBUFFERPROC)gl_funcs[10])
#define oglBufferData				((PFNGLBUFFERDATAPROC)gl_funcs[11])
#define oglVertexAttribPointer		((PFNGLVERTEXATTRIBPOINTERPROC)gl_funcs[12])
#define oglEnableVertexAttribArray	((PFNGLENABLEVERTEXATTRIBARRAYPROC)gl_funcs[13])
#define oglUseProgram				((PFNGLUSEPROGRAMPROC)gl_funcs[14])
#define oglDisableVertexAttribArray ((PFNGLDISABLEVERTEXATTRIBARRAYPROC)gl_funcs[15])
#define oglDeleteVertexArrays		((PFNGLDELETEVERTEXARRAYSPROC)gl_funcs[16])
#define oglBindFragDataLocation		((PFNGLBINDFRAGDATALOCATIONPROC)gl_funcs[17])
#define oglActiveShaderProgram		((PFNGLACTIVESHADERPROGRAMPROC)gl_funcs[18])
#define oglGetUniformLocation		((PFNGLGETUNIFORMLOCATIONPROC)gl_funcs[19])
#define oglProgramUniformMatrix4fv	((PFNGLPROGRAMUNIFORMMATRIX4FVPROC)gl_funcs[20])
#define oglProgramUniformMatrix3fv	((PFNGLPROGRAMUNIFORMMATRIX3FVPROC)gl_funcs[21])
#define oglCreateProgram			((PFNGLCREATEPROGRAMPROC)gl_funcs[22])
#define oglCreateShader				((PFNGLCREATESHADERPROC)gl_funcs[23])
#define oglShaderSource				((PFNGLSHADERSOURCEPROC)gl_funcs[24])
#define oglCompileShader			((PFNGLCOMPILESHADERPROC)gl_funcs[25])
#define oglAttachShader				((PFNGLATTACHSHADERPROC)gl_funcs[26])
#define oglLinkProgram				((PFNGLLINKPROGRAMPROC)gl_funcs[27])
#define oglBindAttribLocation		((PFNGLBINDATTRIBLOCATIONPROC)gl_funcs[28])
#define oglGetShaderiv				((PFNGLGETSHADERIVPROC)gl_funcs[29])
#define oglGetShaderInfoLog			((PFNGLGETSHADERINFOLOGPROC)gl_funcs[30])
#define oglUniformMatrix4fv			((PFNGLUNIFORMMATRIX4FVPROC)gl_funcs[31])
#define oglUniformMatrix3fv			((PFNGLUNIFORMMATRIX3FVPROC)gl_funcs[32])
#define oglDeleteBuffers			((PFNGLDELETEBUFFERSPROC)gl_funcs[33])
#define oglDetachShader				((PFNGLDETACHSHADERPROC)gl_funcs[34])
#define oglDeleteShader				((PFNGLDELETESHADERPROC)gl_funcs[35])
#define oglDeleteProgram			((PFNGLDELETEPROGRAMPROC)gl_funcs[36])
#define oglUniform1f				((PFNGLUNIFORM1FPROC)gl_funcs[37])
#define oglBufferSubData			((PFNGLBUFFERSUBDATAPROC)gl_funcs[38])
#define oglVertexAttribDivisor		((PFNGLVERTEXATTRIBDIVISORPROC)gl_funcs[39])
#define oglDrawArraysInstanced		((PFNGLDRAWARRAYSINSTANCEDPROC)gl_funcs[40])
#define oglIsVertexArray			((PFNGLISVERTEXARRAYPROC)gl_funcs[41])



static inline int gl_program_was_linked(int id) {
	int result;
	char info[1536];
	oglGetProgramiv(id, GL_LINK_STATUS, &result);
	oglGetProgramInfoLog(id, 1024, nullptr, (char *)info);
	if (!result) {
		OUTPUT_ERROR("OpenGL initialization error : %s\n", info);
	}
	return result;
}

static inline int gl_shader_was_compiled(int id) {
	int result;
	char info[1536];
	oglGetShaderiv(id, GL_COMPILE_STATUS, &result);
	oglGetShaderInfoLog(id, 1024, nullptr, (char *)info);
	if (!result) {
		OUTPUT_ERROR("OpenGL initialization error : %s\n", info);
	}
	return result;
}

static inline const char * _gl_get_err_string(GLenum type) {
	switch (type) {
		case GL_NO_ERROR: return "GL_NO_ERROR ";
		case GL_INVALID_ENUM: return "GL_INVALID_ENUM ";
		case GL_INVALID_VALUE: return "GL_INVALID_VALUE ";
		case GL_INVALID_OPERATION: return "GL_INVALID_OPERATION ";
		case GL_INVALID_FRAMEBUFFER_OPERATION: return "GL_INVALID_FRAMEBUFFER_OPERATION ";
		case GL_OUT_OF_MEMORY: return "GL_OUT_OF_MEMORY ";
		case GL_STACK_UNDERFLOW: return "GL_STACK_UNDERFLOW "; // Until OpenGL 4.2
		case GL_STACK_OVERFLOW: return "GL_STACK_OVERFLOW "; // Until OpenGL 4.2
		default: return "invalid error number ";
	}
}

static inline void gl_error_string(char* msg) {
	GLenum err;
	while ((err = glGetError()) != GL_NO_ERROR) {
		strcat_s(msg, 256, _gl_get_err_string(err));
	}
}

static inline int gl_init_funcs() {
	for (int i = 0; i < gl_num_funcs; ++i) {
		gl_funcs[i] = wglGetProcAddress(gl_func_names[i]);
		if (!gl_funcs[i]) {
			OUTPUT_ERROR("Problem getting OpenGL extension function : %s was null.", gl_func_names[i]);
			return 0;
		}
	}
	return 1;
}
