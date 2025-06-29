#include "globals.h"

#define CODE(...) #__VA_ARGS__

/* Uniforms */
struct uniform_info {
	const char* name;
	GLint id;
};

const int u_time = 0; //uniforms[0]
const int u_model = 1; //uniforms[1]
const int u_view = 2; //uniforms[2]
const int u_proj = 3; //uniforms[3]
const int u_transform = 4; //uniforms[4]

static uniform_info uniforms[] = {
	  { "time", -1 }
	, { "model_mat", -1 }
	, { "view_mat", -1 }
	, { "proj_mat", -1 }
	, { "transform_mat", -1 }
};
static const size_t uniforms_num = sizeof(uniforms) / sizeof(uniform_info);

static void gl_init_uniforms(uniform_info arr[], const size_t s, const GLuint pipeline_id) {
	for (int i = 0; i < s; ++i) {
		arr[i].id = oglGetUniformLocation(pipeline_id, arr[i].name);
		if (arr[i].id == -1) {
			OUTPUT_ERROR("Couldnt get uniform location : %s", arr[i].name);
		}
	}
}

/* Locations */
struct location_info {
	const char* name;
	GLuint id;
};

const int loc_vert_pos = 0;
const int loc_transform_pos = 1;
const int loc_transform_rot = 2;
const int loc_transform_scale = 3;

static location_info locations[] = {
	  { "vert_pos", 0 }
	, { "transform_pos", 0 }
	, { "transform_rot", 0 }
	, { "transform_scale", 0 }
};
static const size_t locations_num = sizeof(locations) / sizeof(location_info);

static void gl_bind_locations(const location_info arr[], const size_t s, const GLuint pipeline_id) {
	for (int i = 0; i < s; ++i) {
		oglBindAttribLocation(pipeline_id, i, arr[i].name);
	}
}

static void gl_delete_locations(location_info arr[], const size_t s, const GLuint pipeline_id) {
	for (int i = 0; i < s; ++i) {
		oglDeleteBuffers(1, &arr[i].id);
	}
}

static const char* vert_shader_src = CODE(
	#version 450 core\n
	layout (location = 0)	in vec3 vert_pos;
	layout (location = 1)	in vec3 transform_pos;
	layout (location = 2)	in vec3 transform_rot;
	layout (location = 3)	in vec3 transform_scale;

	uniform float time;
	uniform mat4 model_mat;
	uniform mat4 view_mat;
	uniform mat4 proj_mat;
	uniform mat3 transform_mat;

	out gl_PerVertex {
		vec4 gl_Position;
	};

	// Using a 3x3 matrix.
	mat4 calc_pos_mat3() {
		float cos_a = cos(transform_mat[1][0]);
		float cos_b = cos(transform_mat[1][1]);
		float cos_g = cos(transform_mat[1][2]);
		float sin_a = sin(transform_mat[1][0]);
		float sin_b = sin(transform_mat[1][1]);
		float sin_g = sin(transform_mat[1][2]);

		mat4 t = mat4(
			1.f, 0.f, 0.f, 0.f,
			0.f, 1.f, 0.f, 0.f,
			0.f, 0.f, 1.f, 0.f,
			transform_mat[0][0], transform_mat[0][1], transform_mat[0][2], 1.f
		);
		mat4 r = mat4(
			cos_b*cos_g, cos_b*sin_g, -sin_b, 0.f, 
			cos_g*sin_a*sin_b - cos_a*sin_g, cos_a*cos_g + sin_a*sin_b*sin_g, cos_b*sin_a, 0.f, 
			cos_a*cos_g*sin_b + sin_a*sin_g, -cos_g*sin_a + cos_a*sin_b*sin_g, cos_a*cos_b, 0.f, 
			0.f, 0.f, 0.f, 1.f
		);
		mat4 s = mat4(
			transform_mat[2][0], 0.f, 0.f, 0.f,
			0.f, transform_mat[2][1], 0.f, 0.f,
			0.f, 0.f, transform_mat[2][2], 0.f,
			0.f, 0.f, 0.f, 1.f
		);
		return t * r * s;
	}

	// Using pos, rot, scale vec3
	mat4 calc_pos_vec3() {
		float cos_a = cos(transform_rot[0]);
		float cos_b = cos(transform_rot[1]);
		float cos_g = cos(transform_rot[2]);
		float sin_a = sin(transform_rot[0]);
		float sin_b = sin(transform_rot[1]);
		float sin_g = sin(transform_rot[2]);

		mat4 t = mat4(
			1.f, 0.f, 0.f, 0.f,
			0.f, 1.f, 0.f, 0.f,
			0.f, 0.f, 1.f, 0.f,
			transform_pos[0], transform_pos[1], transform_pos[2], 1.f
		);
		mat4 r = mat4(
			cos_b*cos_g, cos_b*sin_g, -sin_b, 0.f, 
			cos_g*sin_a*sin_b - cos_a*sin_g, cos_a*cos_g + sin_a*sin_b*sin_g, cos_b*sin_a, 0.f, 
			cos_a*cos_g*sin_b + sin_a*sin_g, -cos_g*sin_a + cos_a*sin_b*sin_g, cos_a*cos_b, 0.f, 
			0.f, 0.f, 0.f, 1.f
		);
		mat4 s = mat4(
			transform_scale[0], 0.f, 0.f, 0.f,
			0.f, transform_scale[1], 0.f, 0.f,
			0.f, 0.f, transform_scale[2], 0.f,
			0.f, 0.f, 0.f, 1.f
		);
		return t * r * s;
	}

	void main() {
		//gl_Position = (proj_mat * view_mat * model_mat) * vec4(vert_position, 1.f);
		//vec4 p = vec4(vert_position.x + sin(time), vert_position.yz, 1.f);
		vec4 p = vec4(vert_pos, 1.f);
		gl_Position = (proj_mat * view_mat * model_mat * calc_pos_vec3()) * p;
		//gl_Position = vec4(vert_position, 1.f);
	}\0
);

static const char* frag_shader_src = CODE(
	#version 450 core\n
	layout (location = 0)	out vec4 frag_color;

	void main() {
		frag_color = vec4(0.86, 0.62, 0.86, 0.5f);
	}\0
);

