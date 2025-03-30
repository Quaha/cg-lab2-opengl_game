#version 460 core

layout (location = 0) in vec3 a_position;
layout (location = 1 ) in vec2 a_texture_coord;

out vec2 texture_coord;

//Uniform-variables
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {
	gl_Position = vec4(a_position, 1.0) * model * view * projection;
	texture_coord = a_texture_coord;
}
