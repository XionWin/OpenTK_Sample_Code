#version 330

out vec4 outputColor;

in vec3 color;
in vec2 texCoord;

uniform sampler2D aTexture;

void main()
{
    outputColor = texture(aTexture, texCoord) * vec4(color, 1.0);
}