#version 460 core

out vec4 frag_color;
in vec2 texture_coord;

in vec3 surfaceNormal;
in vec3 toLightVector;

uniform sampler2D texture0;
uniform vec3 lightColour;

void main() {

	vec3 unitNormal = normalize(surfaceNormal);
	vec3 unitLightVector = normalize(toLightVector);

	float nDotl = dot(unitNormal, unitLightVector);
	float brightness = max(nDotl, 0.0);
	vec3 diffuse = brightness * lightColour;

	frag_color = vec4(diffuse, 1.0) * texture(texture0, texture_coord);
}