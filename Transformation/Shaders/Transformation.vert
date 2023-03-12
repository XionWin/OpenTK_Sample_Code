#version 330 core

// the position variable has attribute position 0
layout(location = 0) in vec2 aPosition;  

// This is where the color values we assigned in the main program goes to

layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord; // output a color to the fragment shader

uniform vec2 aCenter;

uniform vec3 aViewport; 

uniform mat4 aTransform;  

void main(void)
{
	mat4 transform = aTransform;
	transform[0][3] = (aTransform[0][3] - aViewport.x / 2.0 + aCenter.x) / aViewport.x * 2.0;
	transform[1][3] = 0.0 - (aTransform[1][3]- aViewport.y / 2.0 + aCenter.y) / aViewport.y * 2.0;
    gl_Position = vec4((aPosition.x - aCenter.x) / aViewport.x * 2.0, 0.0 - (aPosition.y - aCenter.y) / aViewport.y * 2.0, 0.0, 1.0) * transform;

	texCoord = aTexCoord;
}