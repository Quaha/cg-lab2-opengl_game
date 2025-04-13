#version 460 core

in vec2 texture_coord;
in vec3 normal;
in vec3 frag_pos;

out vec4 frag_color;

uniform sampler2D texture0;

uniform vec3 light_positions[8];
uniform vec3 light_colors[8];
uniform float light_power[8];
uniform int light_count;

void main() {
    vec3 unit_normal = normalize(normal);
    vec3 total_diffuse = vec3(0.0);
    
    float a = 0.12;
    float b = 0.02;
    float c = 0.5;

    for(int i = 0; i < light_count; i++) {
        vec3 light_dir = light_positions[i] - frag_pos;
        float distance = length(light_dir);
        light_dir = normalize(light_dir);

        float attenuation = 1.0 / (a * distance * distance + b * distance + c);

        float brightness = max(dot(unit_normal, light_dir), 0.0);
        total_diffuse += brightness * light_colors[i] * light_power[i] * attenuation;
    }
    
    frag_color = vec4(total_diffuse, 1.0) * texture(texture0, texture_coord);
}