#version 330

out vec4 outputColor;

in vec2 texCoord;

uniform sampler2D texture0;

void main()
{
    outputColor = vec4(1.0, 1.0, 1.0, texture(texture0, texCoord)[0]);
}