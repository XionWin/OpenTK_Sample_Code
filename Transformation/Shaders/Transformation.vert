#version 330 core

// the position variable has attribute position 0
layout(location = 0) in vec2 aPos;  

layout(location = 1) in vec3 aColor;

layout(location = 2) in vec2 aTexCoord;

uniform vec2 aCenter;

uniform vec3 aViewport; 

uniform mat4 aTransform;  

out vec3 color;
out vec2 texCoord;

void main(void)
{

	mat4 m1 = mat4(aViewport.x / aViewport.y, 0, 0, 0,	0, 1, 0, 0,		0, 0, 1, 0,		0, 0, 0, 1);
	
	mat4 transform = m1 * aTransform;

	mat4 m2 = mat4(aViewport.y / aViewport.x, 0, 0, 0,	0, 1, 0, 0,		0, 0, 1, 0,		0, 0, 0, 1);
	
	mat4 transform2 = transform * m2;
	transform2[0][3] = (aTransform[0][3] - aViewport.x / 2.0 + aCenter.x) / aViewport.x * 2.0;
	transform2[1][3] = 0.0 - (aTransform[1][3]- aViewport.y / 2.0 + aCenter.y) / aViewport.y * 2.0;
    gl_Position = vec4((aPos.x - aCenter.x) / aViewport.x * 2.0, 0.0 - (aPos.y - aCenter.y) / aViewport.y * 2.0, 0.0, 1.0) * transform2;
	
	color = aColor;
	texCoord = aTexCoord;
}