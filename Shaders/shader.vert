#version 460 core

layout (location = 0) in vec3 a_position;
layout (location = 1 ) in vec2 a_texture_coord;
layout (location = 2 ) in vec3 normal;

out vec2 texture_coord;
out vec3 surfaceNormal;
out vec3 toLightVector;

//Uniform-variables
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

uniform vec3 lightPosition;

void main() {

	vec4 worldPosition = vec4(a_position, 1.0) * model;
	gl_Position = worldPosition * view * projection;
	texture_coord = a_texture_coord;

	surfaceNormal = (model * vec4(normal,0.0)).xyz;
	toLightVector = lightPosition - worldPosition.xyz;
}
