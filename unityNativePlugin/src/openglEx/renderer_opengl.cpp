#include <cstdio>
#include <cstdlib>
#include "renderer_opengl.h"
#include "shader.h"
#include "globals.h"

Renderer::Renderer(GLsizei particle_qty)
	: _data(particle_qty)
{
	if (particle_qty == 0) {
		OUTPUT_ERROR("Please set the quantity of particles desired.\n");
	}
	if (!gl_init_funcs()) {
		OUTPUT_ERROR("Could not get OpenGL functions.\n");
	}
}

Renderer::~Renderer()
{}

void Renderer::init_opengl() {
	OUTPUT_MSG("OpenGL version : %s", glGetString(GL_VERSION));

	/* Less fun shader compiling. */
	_pipeline_id = oglCreateProgram();
	_frag_shader_id = oglCreateShader(GL_FRAGMENT_SHADER);
	oglShaderSource(_frag_shader_id, 1, &frag_shader_src, nullptr);
	oglCompileShader(_frag_shader_id);

	_vert_shader_id = oglCreateShader(GL_VERTEX_SHADER);
	oglShaderSource(_vert_shader_id, 1, &vert_shader_src, nullptr);
	oglCompileShader(_vert_shader_id);

	oglAttachShader(_pipeline_id, _vert_shader_id);
	oglAttachShader(_pipeline_id, _frag_shader_id);

	// bind the attribute locations (inputs)
	//gl_bind_locations(locations, locations_num, _pipeline_id); // Not needed when layout pos provided in shader.

	// bind the FragDataLocation (output)
	oglBindFragDataLocation(_pipeline_id, 0, "frag_color");

	oglLinkProgram(_pipeline_id);
	GL_CHECK_ERROR();

	int was_init = 1;
	was_init *= gl_shader_was_compiled(_vert_shader_id);
	was_init *= gl_shader_was_compiled(_frag_shader_id);
	was_init *= gl_program_was_linked(_pipeline_id);
	if (!was_init) {
		OUTPUT_ERROR("Problem initializing OpenGL. Will not render anything.\n");
		return;
	}

	gl_init_uniforms(uniforms, uniforms_num, _pipeline_id);

	GL_CHECK_ERROR();
	OUTPUT_MSG("%p, %p", wglGetCurrentContext(), wglGetCurrentDC());

	InitVAO();
}

void Renderer::detroy_opengl() {
	OUTPUT_MSG("destroy OpenGL\n");
	oglDeleteVertexArrays(1, &_vertex_array_id);
	gl_delete_locations(locations, locations_num, _pipeline_id);

	oglDetachShader(_pipeline_id, _vert_shader_id);
	oglDetachShader(_pipeline_id, _frag_shader_id);
	oglDeleteShader(_vert_shader_id);
	oglDeleteShader(_frag_shader_id);
	oglDeleteProgram(_pipeline_id);
}

void Renderer::update(float time, float delta_time) {
	//OUTPUT_MSG("Updating");
	float r = 0.f;

	/* Speed */
	for (int i = 0; i < _data.size; ++i) {
		_data.speed[i] += _data.gravity * delta_time * _data.global_speed;
	}

	/* Position */
	for (int i = 0; i < _data.size; ++i) {
		if (_data.pos[i].y > _y_extent && _data.speed[i].y > 0.f) {
			_data.speed[i].y = -_data.speed[i].y;
		} else if (_data.pos[i].y < -_y_extent && _data.speed[i].y < 0.f) {
			_data.speed[i].y = -_data.speed[i].y;
		}
		_data.pos[i] += _data.speed[i] * delta_time;
	}

	/* Rotation */
	for (int i = 0; i < _data.size; ++i) {
		_data.rot[i].x += 0.1f;
		_data.rot[i].y += 0.0001f;
	}

	/* Scale */
	//for (int i = 0; i < _data.size; ++i) {
	//	_data.scale[i].x = r;
	//	_data.scale[i].y = r;
	//}

}

void Renderer::render(float time, float delta_time) {
	//OUTPUT_MSG("Rendering");

	/* Unity will drop VAO after 1-2 frames. After that we are fine. */
	if (oglIsVertexArray(_vertex_array_id) == GL_FALSE) {
		OUTPUT_MSG("Recreating VAO");
		InitVAO();
	}

	/* Basic render state. */
	glDisable(GL_CULL_FACE);
	glDepthFunc(GL_LEQUAL);
	glEnable(GL_DEPTH_TEST);
	glDepthMask(GL_FALSE);
	glEnable(GL_BLEND);
	glBlendFunc(GL_SRC_ALPHA, GL_ONE_MINUS_SRC_ALPHA);

	oglUseProgram(_pipeline_id);
	oglUniformMatrix4fv(uniforms[u_model].id, 1, GL_FALSE, _model_mat);
	oglUniformMatrix4fv(uniforms[u_view].id, 1, GL_FALSE, _view_mat);
	oglUniformMatrix4fv(uniforms[u_proj].id, 1, GL_FALSE, _projection_mat);
	oglUniform1f(uniforms[u_time].id, time);

	BindAndFillVBOWithVAO();
	oglBindVertexArray(_vertex_array_id);
	oglDrawArraysInstanced(GL_TRIANGLE_STRIP, 0, 4, _data.size);
	oglBindVertexArray(0);

	oglDisableVertexAttribArray(loc_vert_pos);
	oglDisableVertexAttribArray(loc_transform_pos);
	oglDisableVertexAttribArray(loc_transform_rot);
	oglDisableVertexAttribArray(loc_transform_scale);

	GL_CHECK_ERROR();
}

void Renderer::InitVAO()
{
	oglGenVertexArrays(1, &_vertex_array_id);
	oglBindVertexArray(_vertex_array_id);

	oglGenBuffers(1, &locations[loc_vert_pos].id);
	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_vert_pos].id);
	oglBufferData(GL_ARRAY_BUFFER, sizeof(quad),
			quad, GL_STATIC_DRAW);
	oglEnableVertexAttribArray(loc_vert_pos);
	oglVertexAttribPointer(loc_vert_pos, 3, GL_FLOAT, GL_FALSE, 0, reinterpret_cast<void*>(0));

	/* Use 3 separate buffers to map directly to the data arrays. */
	oglGenBuffers(1, &locations[loc_transform_pos].id);
	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_transform_pos].id);
	oglBufferData(GL_ARRAY_BUFFER, _data.size * sizeof(vec3),
			nullptr, GL_STREAM_DRAW);
	oglEnableVertexAttribArray(loc_transform_pos);
	oglVertexAttribPointer(loc_transform_pos, 3, GL_FLOAT, GL_FALSE, 0, reinterpret_cast<void*>(0));
	oglVertexAttribDivisor(loc_transform_pos, 1);

	oglGenBuffers(1, &locations[loc_transform_rot].id);
	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_transform_rot].id);
	oglBufferData(GL_ARRAY_BUFFER, _data.size * sizeof(vec3),
			nullptr, GL_STREAM_DRAW);
	oglEnableVertexAttribArray(loc_transform_rot);
	oglVertexAttribPointer(loc_transform_rot, 3, GL_FLOAT, GL_FALSE, 0, reinterpret_cast<void*>(0));
	oglVertexAttribDivisor(loc_transform_rot, 1);

	oglGenBuffers(1, &locations[loc_transform_scale].id);
	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_transform_scale].id);
	oglBufferData(GL_ARRAY_BUFFER, _data.size * sizeof(vec3),
			nullptr, GL_STREAM_DRAW);
	oglEnableVertexAttribArray(loc_transform_scale);
	oglVertexAttribPointer(loc_transform_scale, 3, GL_FLOAT, GL_FALSE, 0, reinterpret_cast<void*>(0));
	oglVertexAttribDivisor(loc_transform_scale, 1);

	oglBindVertexArray(0);
	GL_CHECK_ERROR();
}

/* All we need is to push data. */
void Renderer::BindAndFillVBOWithVAO()
{
	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_transform_pos].id);
	oglBufferData(GL_ARRAY_BUFFER, _data.size * sizeof(vec3), nullptr, GL_STREAM_DRAW); // Buffer orphaning, a common way to improve streaming perf.
	oglBufferSubData(GL_ARRAY_BUFFER, 0, _data.size * sizeof(vec3), _data.pos);

	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_transform_rot].id);
	oglBufferData(GL_ARRAY_BUFFER, _data.size * sizeof(vec3), nullptr, GL_STREAM_DRAW);
	oglBufferSubData(GL_ARRAY_BUFFER, 0, _data.size * sizeof(vec3), _data.rot);

	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_transform_scale].id);
	oglBufferData(GL_ARRAY_BUFFER, _data.size * sizeof(vec3), nullptr, GL_STREAM_DRAW);
	oglBufferSubData(GL_ARRAY_BUFFER, 0, _data.size * sizeof(vec3), _data.scale);
}

/* If we aren't using VAOs. */
void Renderer::BindAndFillVBOWithoutVAO(bool enable_attrib_divisor)
{
	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_vert_pos].id);
	oglEnableVertexAttribArray(loc_vert_pos);
	oglVertexAttribPointer(loc_vert_pos, 3, GL_FLOAT, GL_FALSE, 0, reinterpret_cast<void*>(0));

	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_transform_pos].id);
	oglBufferData(GL_ARRAY_BUFFER, _data.size * sizeof(vec3), nullptr, GL_STREAM_DRAW); // Buffer orphaning, a common way to improve streaming perf.
	oglBufferSubData(GL_ARRAY_BUFFER, 0, _data.size * sizeof(vec3), _data.pos);
	oglEnableVertexAttribArray(loc_transform_pos);
	oglVertexAttribPointer(loc_transform_pos, 3, GL_FLOAT, GL_FALSE, 0, reinterpret_cast<void*>(0));
	if (enable_attrib_divisor) {
		oglVertexAttribDivisor(loc_transform_pos, 1);
	}

	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_transform_rot].id);
	oglBufferData(GL_ARRAY_BUFFER, _data.size * sizeof(vec3), nullptr, GL_STREAM_DRAW);
	oglBufferSubData(GL_ARRAY_BUFFER, 0, _data.size * sizeof(vec3), _data.rot);
	oglEnableVertexAttribArray(loc_transform_rot);
	oglVertexAttribPointer(loc_transform_rot, 3, GL_FLOAT, GL_FALSE, 0, reinterpret_cast<void*>(0));
	if (enable_attrib_divisor) {
		oglVertexAttribDivisor(loc_transform_rot, 1);
	}

	oglBindBuffer(GL_ARRAY_BUFFER, locations[loc_transform_scale].id);
	oglBufferData(GL_ARRAY_BUFFER, _data.size * sizeof(vec3), nullptr, GL_STREAM_DRAW);
	oglBufferSubData(GL_ARRAY_BUFFER, 0, _data.size * sizeof(vec3), _data.scale);
	oglEnableVertexAttribArray(loc_transform_scale);
	oglVertexAttribPointer(loc_transform_scale, 3, GL_FLOAT, GL_FALSE, 0, reinterpret_cast<void*>(0));
	if (enable_attrib_divisor) {
		oglVertexAttribDivisor(loc_transform_scale, 1);
	}
}

/* Naive pushing to uniforms. */
void Renderer::DrawDataWithUniforms()
{
	for (int i = 0; i < _data.size; ++i) {
		GLfloat t[16] = {
			_data.pos[i].x, _data.pos[i].y, _data.pos[i].z
			, _data.rot[i].x, _data.rot[i].y, _data.rot[i].z
			, _data.scale[i].x, _data.scale[i].y, _data.scale[i].z
		};
		oglUniformMatrix3fv(uniforms[u_transform].id, 1, GL_FALSE, t);

		glDrawArrays(GL_TRIANGLE_STRIP, GL_POINTS, 4);
	}
}
