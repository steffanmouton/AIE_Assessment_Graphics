using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using LearnOpenTK.Common;

namespace LearnOpenTK
{
    // In this chapter we will focus on how to use lighting to make our games and other applications more lifelike

    // In this first part the focus will mainly be on setting up a scene for testing the different coloring options.
    // We draw two cubes one at 0,0,0 for testing our light shader, the second one is drawn where we have the light.
    // Furthermore in the shaders we have set up some basic physically based coloring.
    public class Window : GameWindow
    {
        // The vertices are now used to draw cubes.
        // For this example, we aren't using texture coordinates.
        // You can use textures with lighting (and we will get onto this), but for simplicity's sake, we'll just use a solid color here
        private readonly float[] _vertices =
        {
            // Position
            -0.5f, -0.5f, -0.5f, // Front face
             0.5f, -0.5f, -0.5f, 
             0.5f,  0.5f, -0.5f, 
             0.5f,  0.5f, -0.5f, 
            -0.5f,  0.5f, -0.5f, 
            -0.5f, -0.5f, -0.5f,
            
            -0.5f, -0.5f,  0.5f, // Back face
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,
            
            -0.5f,  0.5f,  0.5f, // Left face
            -0.5f,  0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f, -0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            
             0.5f,  0.5f,  0.5f, // Right face
             0.5f,  0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
             
            -0.5f, -0.5f, -0.5f, // Bottom face
             0.5f, -0.5f, -0.5f,
             0.5f, -0.5f,  0.5f,
             0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f,  0.5f,
            -0.5f, -0.5f, -0.5f,
            
            -0.5f,  0.5f, -0.5f, // Top face
             0.5f,  0.5f, -0.5f,
             0.5f,  0.5f,  0.5f,
             0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f,  0.5f,
            -0.5f,  0.5f, -0.5f
        };
        
        // This is the position of both the light and the place the lamp cube will be drawn in the scene
        private readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);

        private int _vertexBufferObject;
        
        // I renamed the vertex array object since we now want to VAO's one for the model (the big cube for testing light shaders),
        // and one for the lamp so we can see where the light source comes from.
        // In an actual application you would probably either dont draw the lamp at all or draw it with a model of a lamp of some sort.
        private int _vaoModel;
        private int _vaoLamp;

        // We also need two shaders, one for the lamp and one for our lighting material.
        // The lighting shader is where most of this chapter will take place as this is where a lot of the lighting "magic" happens.
        private Shader _lampShader;
        private Shader _lightingShader;
        
        private Camera _camera;
        private bool _firstMove = true;
        private Vector2 _lastPos;

        public Window(int width, int height, string title) : base(width, height, GraphicsMode.Default, title) { }


        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            // Load the two different shaders, they use the same vertex shader program. However they have two different fragment shaders.
            // This is because the lamp only uses a basic shader to turn it white, it wouldn't make sense to have the lamp lit in other colors.
            // The lighting shaders uses the lighting.frag shader which is what a large part of this chapter will be about
            _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");

            // Initialize the vao for the model
            _vaoModel = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            var vertexLocation = _lightingShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);


            // Initialize the vao for the lamp, this is mostly the same as the code for the model cube
            _vaoLamp = GL.GenVertexArray();
            GL.BindVertexArray(_vaoLamp);
            
            // We only need to bind to the VBO, the container's VBO's data already contains the correct data.
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            
            // Set the vertex attributes (only position data for our lamp)
            vertexLocation = _lampShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);

            _camera = new Camera(Vector3.UnitZ * 3, Width / (float) Height);
            
            CursorVisible = false;
            
            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Draw the model/cube with the lighting shader
            GL.BindVertexArray(_vaoModel);

            _lightingShader.Use();
            
            // Matrix4.Identity is used as the matrix, since we just want to draw it at 0, 0, 0
            _lightingShader.SetMatrix4("model", Matrix4.Identity); 
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            
            _lightingShader.SetVector3("objectColor", new Vector3(1.0f, 0.5f, 0.31f));
            _lightingShader.SetVector3("lightColor", new Vector3(1.0f, 1.0f, 1.0f));

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            // Draw the lamp, this is mostly the same as for the model cube
            GL.BindVertexArray(_vaoModel);
            
            _lampShader.Use();

            Matrix4 lampMatrix = Matrix4.Identity;
            lampMatrix *= Matrix4.CreateScale(0.2f); // We scale the lamp cube down a bit to make it less dominant
            lampMatrix *= Matrix4.CreateTranslation(_lightPos);
            
            _lampShader.SetMatrix4("model", lampMatrix);
            _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

            SwapBuffers();

            base.OnRenderFrame(e);
        }

        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!Focused)
            {
                return;
            }

            var input = Keyboard.GetState();

            if (input.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;
            
            if (input.IsKeyDown(Key.W))
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward 
            if (input.IsKeyDown(Key.S))
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            if (input.IsKeyDown(Key.A))
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            if (input.IsKeyDown(Key.D))
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            if (input.IsKeyDown(Key.Space))
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up 
            if (input.IsKeyDown(Key.LShift))
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down

            var mouse = Mouse.GetState();

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);
                
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity;
            }
            
            base.OnUpdateFrame(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            if (Focused)
            {
                Mouse.SetPosition(X + Width/2f, Y + Height/2f);
            }
            
            base.OnMouseMove(e);
        }
        
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            _camera.Fov -= e.DeltaPrecise;
            base.OnMouseWheel(e);
        }

        
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            _camera.AspectRatio = Width / (float)Height;
            base.OnResize(e);
        }


        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_vaoModel);
            GL.DeleteVertexArray(_vaoLamp);

            GL.DeleteProgram(_lampShader.Handle);
            GL.DeleteProgram(_lightingShader.Handle);

            base.OnUnload(e);
        }
    }
}