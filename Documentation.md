#### Steffan Mouton
#### s188045
#### 31 October 2019

# Using OpenTK to create an OpenGL Scene

We were given OpenTK to use as a framework for generating OpenGL projets. We created everything from scratch following the tutorials, but for this assessment it was decided we could use the complete version of OpenTK.

We are using this framework to demonstrate the functionality of a graphics pipeline.


## Pipeline Explanation with Code Structure

We first need to feed a series of vertices into an array. This stores the locations of each point to be rendered, along with its matching normal and texture coordinates. These will be referenced later by the shaders.

``` C#
private readonly float[] _vertices =
        {
            // Positions          Normals              Texture coords
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
        
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f,
        
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
            -0.5f, -0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f,  0.5f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
        
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f,
        
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
             0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
             0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f, -0.5f,  0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f, -0.5f, -0.5f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f,
        
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
             0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f,
            -0.5f,  0.5f,  0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f
        };
```
We then declare many variables which will be used in the OnLoad function, which follows:

In the first portion of this function, we declare the background color of the window and clear to it.
We then generate buffer objects that store the vertex data we provided earlier, but in a way that the shaders can understand and use.
``` C++
protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
```

All of the following code is used to dictate to the program exactly how to read the large system of vertices that we originall fed it. Via defining stride, length of vertices, etc., we can tell the buffers how to read it: I.e, "Each vertex contains 8 points. The first 3 define a vector 3, which is the position of the point. The next 3 points determine the normal mapping at that point. The final 2 points define what part of the assigned texture lies upon this point."
We also set the camera location at the end.

```C++
            // Our two textures are loaded in from memory, you should head over and
            // check them out and compare them to the results.
            
            // THIS DOESN'T WORK AND I HAVE NO CLUE WHY ?!?!?!?!?!!? help
            //_diffuseMap = new Texture("Resources/mud_color.png");
            //_specularMap = new Texture("Resources/mud_spec.png");
            
            //Rename whatever image you want to display to container2.png like a monster.
            _diffuseMap = new Texture("Resources/container2.png");
            _specularMap = new Texture("Resources/container2_specular.png");
            
            _vaoModel = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            // All of the vertex attributes have been updated to now have a stride of 8 float sizes.
            var positionLocation = _lightingShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var normalLocation = _lightingShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            
            // The texture coords have now been added too, remember we only have 2 coordinates as the texture is 2d,
            // so the size parameter should only be 2 for the texture coordinates.
            var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            _vaoLamp = GL.GenVertexArray();
            GL.BindVertexArray(_vaoLamp);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            // The lamp shader should have its stride updated aswell, however we dont actually
            // use the texture coords for the lamp, so we dont need to add any extra attributes.
            positionLocation = _lampShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            _camera = new Camera(Vector3.UnitZ * 3, Width / (float) Height);
            
            CursorVisible = false;
            
            base.OnLoad(e);
        }
```

Next, we call all the functions we want to have happen when a frame renders.
We apply our vertex Shaders to every vertex. The specifics of what these shaders to are defined in the shader files themselves. We can change what behaviours act on each vertex via swapping out the shader file text. 


Here is the shader.vert file we are using for this demonstration. You can see that the first segment takes in 3 layouts, which are the defined vertices, split by the stride operations we performed earlier. This works such that every single point in space that we defined has a position, a normal, and matching texture coordinates.

To make this a "custom" shader, we multiply the texture coordinates by 4, which tiles the texture 4 times in each direction. If you look closely at the texture rendered onto the box, you can see this effect.
```GLSL
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
```


Here is the OnRenderFrame section, which is handling the shader files we have defined.
```C++
protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_vaoModel);

            // The two textures need to be used, in this case we use the diffuse map as our 0th texture
            // and the specular map as our 1st texture.
            _diffuseMap.Use();
            _specularMap.Use(TextureUnit.Texture1);
            _lightingShader.Use();
            
            _lightingShader.SetMatrix4("model", Matrix4.Identity);
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            
            _lightingShader.SetVector3("viewPos", _camera.Position);
            
            // Here we specify to the shaders what textures they should refer to when we want to get the positions.
            _lightingShader.SetInt("material.diffuse", 0);
            _lightingShader.SetInt("material.specular", 1);
            _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader.SetFloat("material.shininess", 32.0f);
            
            _lightingShader.SetVector3("light.position", _lightPos);
            _lightingShader.SetVector3("light.ambient",  new Vector3(0.2f));
            _lightingShader.SetVector3("light.diffuse",  new Vector3(0.5f));
            _lightingShader.SetVector3("light.specular", new Vector3(1.0f));

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            GL.BindVertexArray(_vaoModel);
            
            _lampShader.Use();

            Matrix4 lampMatrix = Matrix4.Identity;
            lampMatrix *= Matrix4.CreateScale(0.2f);
            lampMatrix *= Matrix4.CreateTranslation(_lightPos);
            
            _lampShader.SetMatrix4("model", lampMatrix);
            _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            SwapBuffers();

            base.OnRenderFrame(e);
        }
```

There are some optional steps in the OpenGL pipeline that we do not utilize in our shader files here - Tesselation for example.

Next, we enter the vertex post processing stage. We have little to no input here. Importantly, objects outside the bounds of the viewport are clipped from the calculations here.

Then comes Primitive assembly. At this stage, the vertex data is ordered into primitives. We see some examples of this being done in the above function as `GL.DrawArrays(PrimitiveType.Triangles, 0, 36)`.

At this point, the primitives are rasterized to the screen. This is the step in which the perfect primitive shapes that have been handled so far are converted into pixel space on screen. This stage outputs chunks of pixels known as "fragments".

Now we enter the Fragment Shader stage. We take each of the fragments generated by the Rasterization stage, and colors them based on user-written shaders. This dictates the final color of each fragment.

Lastly, there are some Per-sample Operations that can be run if they are enabled, which involve some tests to determine the outputs of all the previous steps. We did not include any here.
