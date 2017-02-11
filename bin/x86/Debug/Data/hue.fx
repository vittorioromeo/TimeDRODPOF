#version 140
#extension GL_ARB_texture_rectangle : enable

uniform sampler2DRect texture;
const mat3 rgb2yiq = mat3(0.299, 0.587, 0.114, 0.595716, -0.274453, -0.321263, 0.211456, -0.522591, 0.311135);
const mat3 yiq2rgb = mat3(1.0, 0.9563, 0.6210, 1.0, -0.2721, -0.6474, 1.0, -1.1070, 1.7046);
uniform float hue;
in vec2 in_texCoord;
out vec2 out_texCoord;
out vec4 out_fragColor;

void main() {

vec3 yColor = rgb2yiq * texture2DRect(texture, in_texCoord.st).rgb; 

float originalHue = atan(yColor.b, yColor.g);
float finalHue = originalHue + hue;

float chroma = sqrt(yColor.b*yColor.b+yColor.g*yColor.g);

vec3 yFinalColor = vec3(yColor.r, chroma * cos(finalHue), chroma * sin(finalHue));
out_fragColor    = vec4(yiq2rgb*yFinalColor, 1.0);
}