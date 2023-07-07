#version 330 core

uniform mat4 projection;

layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_texCoord;
layout(location = 2) in vec4 in_color;

out vec4 color;
out vec2 texCoord;

void main()
{
    gl_Position = projection * vec4(in_position, 0, 1);
    color = in_color;
    texCoord = in_texCoord;
}