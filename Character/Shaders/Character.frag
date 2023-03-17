#version 330

out vec4 outputColor;

in vec3 color;
in vec2 texCoord;

uniform sampler2D aTexture;
uniform vec2 aTexOffset;

void main()
{
    outputColor = texture(aTexture, texCoord + aTexOffset) * vec4(color, 1.0);
}