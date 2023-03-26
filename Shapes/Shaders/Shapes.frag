#version 330

out vec4 outputColor;

in vec3 color;
in vec2 texCoord;

uniform sampler2D aTexture;
uniform vec2 aTexOffset;


uniform int aMode;
void main()
{
    if (aMode == 0)
    {
        outputColor = vec4(color, 1.0);
    }
    else
    {
        outputColor = texture(aTexture, texCoord + aTexOffset) * vec4(color, 1.0);
    }
}