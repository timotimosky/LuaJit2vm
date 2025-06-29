#pragma once
#include <windows.h>
#include <GL/gl.h>
#include <cstdio>
#include "glext.h"

static const int random_seed = 1029831;

struct vec3 {
	GLfloat x;
	GLfloat y;
	GLfloat z;

	inline vec3& operator+=(const vec3& rhs) {
		x += rhs.x;
		y += rhs.y;
		z += rhs.z;
		return *this;
	}
	
	inline friend vec3 operator+(vec3 lhs, const vec3& rhs) {
		return { lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z };
	}

	inline friend vec3 operator*(float lhs, const vec3& rhs) {
		return { lhs * rhs.x, lhs * rhs.y, lhs * rhs.z };
	}

	inline friend vec3 operator*(const vec3& rhs, float lhs) {
		return { lhs * rhs.x, lhs * rhs.y, lhs * rhs.z };
	}
};

static const GLfloat quad[12] = {
	-0.5f, -0.5f, 0.0f,
	 0.5f, -0.5f, 0.0f,
	-0.5f,  0.5f, 0.0f,
	 0.5f,  0.5f, 0.0f,
};

struct ParticleData {
	GLsizei size = 0;

	const vec3 gravity = { 0.f, -9.8f, 0.f };
	const float spread = 1.5f;
	const float global_speed = 0.1f;

	vec3* pos = nullptr;
	vec3* rot = nullptr;
	vec3* scale = nullptr;
	vec3* speed = nullptr;

	ParticleData(GLsizei qty) {
		size = qty;
		pos = (vec3*)malloc(size * sizeof(vec3));
		rot = (vec3*)malloc(size * sizeof(vec3));
		scale = (vec3*)malloc(size * sizeof(vec3));
		speed = (vec3*)malloc(size * sizeof(vec3));

		srand(random_seed); // Intentional, use deterministic randomness. TODO: gpu_rand()
		float r = 0.f;
		vec3 main_dir = { 0.f, 5.f, 0.f };
		for (int i = 0; i < size; ++i) {
			r = (static_cast<float>(rand() % 2000) - 1000.f) / 1000.f;

			pos[i] = { 0.f, 0.f, 0.f };
			rot[i] = {
				((static_cast<float>(rand()) / RAND_MAX) * 2.f - 1.f) * 360.f,
				((static_cast<float>(rand()) / RAND_MAX) * 2.f - 1.f) * 360.f,
				((static_cast<float>(rand()) / RAND_MAX) * 2.f - 1.f) * 360.f 
			};
			scale[i] = { 0.25f + r, 0.25f + r, 1.f };
			speed[i] = main_dir + vec3 {
				(static_cast<float>(rand() % 2000) - 1000.f) / 1000.f,
				(static_cast<float>(rand() % 2000) - 1000.f) / 1000.f,
				(static_cast<float>(rand() % 2000) - 1000.f) / 1000.f
			} * spread;
		}
	}

	~ParticleData() {
		size = 0;
		if (pos) {
			free(pos);
			pos = nullptr;
		}
		if (rot) {
			free(rot);
			rot = nullptr;
		}
		if (scale) {
			free(scale);
			scale = nullptr;
		}
		if (speed) {
			free(speed);
			speed = nullptr;
		}
	}

	void print(size_t i) {
		printf("%f %f %f\n%f %f %f\n%f %f %f\n\n",
			pos[i].x, pos[i].y, pos[i].z,
			rot[i].x, rot[i].y, rot[i].z,
			scale[i].x, scale[i].y, scale[i].z
		);
	}
};

class Renderer {
public:
	Renderer(GLsizei particle_qty);
	~Renderer();

	void init_opengl();
	void detroy_opengl();

	void update(float time, float delta_time);
	void render(float time, float delta_time);

	void mvp(const GLfloat m[16], const GLfloat v[16], const GLfloat p[16]) {
		memcpy(_model_mat, m, sizeof(GLfloat) * 16);
		memcpy(_view_mat, v, sizeof(GLfloat) * 16);
		memcpy(_projection_mat, p, sizeof(GLfloat) * 16);
	}

private:
	void InitVAO();
	void BindAndFillVBOWithVAO();
	void BindAndFillVBOWithoutVAO(bool enable_attrib_divisor = false);
	void DrawDataWithUniforms();

	ParticleData _data;

	GLuint _pipeline_id = 0;
	GLuint _vert_shader_id = 0;
	GLuint _frag_shader_id = 0;
	GLuint _vertex_array_id = 0;

	GLfloat _model_mat[16] = {};
	GLfloat _view_mat[16] = {};
	GLfloat _projection_mat[16] = {};

	/* TODO: Expose these. */
	const float _velocity = 0.01f;
	const float _y_extent = 20.f;
};
