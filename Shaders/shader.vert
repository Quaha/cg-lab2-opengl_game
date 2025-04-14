#version 460 core

layout(location = 0) in vec3 a_position;
layout(location = 1) in vec2 a_texture_coord;
layout(location = 2) in vec3 a_normal;

out vec2 texture_coord;
out vec3 normal;
out vec3 frag_pos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main() {

    vec4 world_pos =  vec4(a_position, 1.0) * model;
    gl_Position = world_pos * view * projection;
    
    frag_pos = world_pos.xyz;
    texture_coord = a_texture_coord;
    normal = a_normal;
}