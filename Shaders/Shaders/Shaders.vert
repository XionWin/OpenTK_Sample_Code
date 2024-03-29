#version 330 core

// the position variable has attribute position 0
layout(location = 0) in vec2 aPos;  

// This is where the color values we assigned in the main program goes to
layout(location = 1) in vec3 aColor;

out vec3 outColor; // output a color to the fragment shader

uniform vec3 aViewport;

void main(void)
{
    gl_Position = vec4(aPos.x / aViewport.x * 2.0 - 1.0, 1.0 - aPos.y / aViewport.y * 2.0, 0.0, 1.0);
    outColor = aColor;
}