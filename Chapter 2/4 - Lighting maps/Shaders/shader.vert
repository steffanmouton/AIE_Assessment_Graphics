#version 330 core
layout (location = 0) in vec3 aPos;
layout (location = 1) in vec3 aNormal;
layout (location = 2) in vec2 aTexCoords;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 Normal;
out vec3 FragPos;
out vec2 TexCoords;

void main()
{
    gl_Position = vec4(aPos, 1.0) * model * view * projection;
    FragPos = vec3(vec4(aPos, 1.0) * model);
    Normal = aNormal * mat3(transpose(inverse(model))) * vec3(.25);
    
	// This is the default setting, to match the texture to the square perfectly.
	//TexCoords = aTexCoords;

	// We want to add some custom effects, such as tiling the texture over the shape.
	// For example, multiplying the coordinates by 4 will tile the texture 4 times in each direction,
	// giving us 16 tiles on one square face.
	TexCoords = aTexCoords * 4;
}