#version 460 core

in vec2 texCoord;
in vec3 normal;
in vec3 fragPos;

out vec4 fragColor;

uniform sampler2D texture0;

uniform vec3 lightPositions[8];
uniform vec3 lightColors[8];
uniform float lightPower[8];
uniform int lightCount;

void main() {
    vec3 unitNormal = normalize(normal);
    vec3 totalDiffuse = vec3(0.0);
    
    float constant = 0.5;
    float linear = 0.02;
    float quadratic = 0.12;

    for(int i = 0; i < lightCount; i++) {
        vec3 lightDir = lightPositions[i] - fragPos;
        float distance = length(lightDir);
        lightDir = normalize(lightDir);

        float attenuation = 1.0 / (constant + linear * distance + quadratic * (distance * distance));

        float brightness = max(dot(unitNormal, lightDir), 0.0);
        totalDiffuse += brightness * lightColors[i] * lightPower[i] * attenuation;
    }
    
    fragColor = vec4(totalDiffuse, 1.0) * texture(texture0, texCoord);

}