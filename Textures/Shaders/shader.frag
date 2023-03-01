#version 330

out vec4 outputColor;

in vec3 outColor;

void main()
{
    outputColor = vec4(outColor, 1.0);
}