#version 330 core

// the position variable has attribute position 0
layout(location = 0) in vec3 aPosition;  

// This is where the color values we assigned in the main program goes to

layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord; // output a color to the fragment shader

uniform vec3 aViewport; 

void main(void)
{
    gl_Position = vec4(aPosition.x / aViewport.x * 2.0 - 1.0, 1.0 - aPosition.y / aViewport.y * 2.0, 0.0, 1.0);

	texCoord = aTexCoord;
}