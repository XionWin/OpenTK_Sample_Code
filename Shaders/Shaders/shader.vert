#version 330 core

// the position variable has attribute position 0
layout(location = 0) in vec3 aPosition;  

// This is where the color values we assigned in the main program goes to
layout(location = 1) in vec3 aColor;

out vec3 outColor; // output a color to the fragment shader

uniform vec3 aViewport; 

void main(void)
{
	// see how we directly give a vec3 to vec4's constructor
    //gl_Position = vec4(aPosition, 1.0);
	
    gl_Position = vec4(aPosition.x / aViewport.x * 2.0 - 1.0, 1.0 - aPosition.y / aViewport.y * 2.0, 0.0, 1.0);

	// We use the outColor variable to pass on the color information to the frag shader
	outColor = aColor;
}