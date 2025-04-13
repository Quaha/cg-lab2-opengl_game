#version 460 core

layout(location = 0) in vec3 a_position;
layout(location = 1) in vec2 a_texCoord;
layout(location = 2) in vec3 a_normal;

out vec2 texCoord;
out vec3 normal;
out vec3 fragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {

    vec4 worldPos =  vec4(a_position, 1.0) * model;
    gl_Position = worldPos * view * projection;
    
    fragPos = worldPos.xyz;
    texCoord = a_texCoord;
    normal = mat3(transpose(inverse(model))) * a_normal;
}