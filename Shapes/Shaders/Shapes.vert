#version 330 core

// the position variable has attribute position 0
layout(location = 0) in vec2 aPos;  

layout(location = 1) in vec3 aColor;

layout(location = 2) in vec2 aTexCoord;

uniform vec2 aCenter;

uniform vec3 aViewport; 

uniform mat3 aTransform;

uniform int aPointSize;

out vec3 color;
out vec2 texCoord;

void main(void)
{

	mat3 m1 = mat3(
		aViewport.x / aViewport.y, 0, 0,
		0, 1, 0,
		0, 0, 1
		);
	
	mat3 transform = m1 * aTransform;

	mat3 m2 = mat3(
		aViewport.y / aViewport.x, 0, 0,
		0, 1, 0,
		0, 0, 1
		);
	
	mat3 transform2 = transform * m2;
	transform2[0][2] = (aTransform[0][2] - aViewport.x / 2.0 + aCenter.x) / aViewport.x * 2.0;
	transform2[1][2] = 0.0 - (aTransform[1][2] - aViewport.y / 2.0 + aCenter.y) / aViewport.y * 2.0;

	vec3 r2d = vec3((aPos.x - aCenter.x) / aViewport.x * 2.0, - (aPos.y - aCenter.y) / aViewport.y * 2.0, 1.0) * transform2;
	gl_Position = vec4(r2d, 1.0);
	
	color = aColor;
	texCoord = aTexCoord;
	gl_PointSize  = aPointSize;
}