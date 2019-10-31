﻿using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using LearnOpenTK.Common;

namespace LearnOpenTK
{
    // In this tutorial we focus on how to set up a scene with multiple lights, both of different types but also
    // with several point lights
    public class Window : GameWindow
    {
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
        
        private readonly Vector3[] _cubePositions =
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(2.0f, 5.0f, -15.0f),
            new Vector3(-1.5f, -2.2f, -2.5f),
            new Vector3(-3.8f, -2.0f, -12.3f),
            new Vector3(2.4f, -0.4f, -3.5f),
            new Vector3(-1.7f, 3.0f, -7.5f),
            new Vector3(1.3f, -2.0f, -2.5f),
            new Vector3(1.5f, 2.0f, -2.5f),
            new Vector3(1.5f, 0.2f, -1.5f),
            new Vector3(-1.3f, 1.0f, -1.5f)
        };
        
        // We need the point lights' positions to draw the lamps and to get light the materials properly
        private readonly Vector3[] _pointLightPositions =
        {
            new Vector3(0.7f, 0.2f, 2.0f),
            new Vector3(2.3f, -3.3f, -4.0f),
            new Vector3(-4.0f, 2.0f, -12.0f),
            new Vector3(0.0f, 0.0f, -3.0f)
        };
        
        private readonly Vector3 _lightPos = new Vector3(1.2f, 1.0f, 2.0f);

        private int _vertexBufferObject;
        private int _vaoModel;
        private int _vaoLamp;

        private Shader _lampShader;
        private Shader _lightingShader;
        private Texture _diffuseMap;
        private Texture _specularMap;
        
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

            _lightingShader = new Shader("Shaders/shader.vert", "Shaders/lighting.frag");
            _lampShader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _diffuseMap = new Texture("Resources/container2.png");
            _specularMap = new Texture("Resources/container2_specular.png");

            _vaoModel = GL.GenVertexArray();
            GL.BindVertexArray(_vaoModel);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            var positionLocation = _lightingShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var normalLocation = _lightingShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            
            var texCoordLocation = _lightingShader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            _vaoLamp = GL.GenVertexArray();
            GL.BindVertexArray(_vaoLamp);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            positionLocation = _lampShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            _camera = new Camera(Vector3.UnitZ * 3, Width / (float) Height);
            
            CursorVisible = false;
            
            base.OnLoad(e);
        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_vaoModel);

            _diffuseMap.Use();
            _specularMap.Use(TextureUnit.Texture1);
            _lightingShader.Use();
            
            _lightingShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lightingShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            
            _lightingShader.SetVector3("viewPos", _camera.Position);
            
            _lightingShader.SetInt("material.diffuse", 0);
            _lightingShader.SetInt("material.specular", 1);
            _lightingShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            _lightingShader.SetFloat("material.shininess", 32.0f);
            
            /*
               Here we set all the uniforms for the 5/6 types of lights we have. We have to set them manually and index 
               the proper PointLight struct in the array to set each uniform variable. This can be done more code-friendly
               by defining light types as classes and set their values in there, or by using a more efficient uniform approach
               by using 'Uniform buffer objects', but that is something we'll discuss in the 'Advanced GLSL' tutorial.
            */
            // Directional light
            _lightingShader.SetVector3("dirLight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
            _lightingShader.SetVector3("dirLight.ambient", new Vector3(0.05f, 0.05f, 0.05f));
            _lightingShader.SetVector3("dirLight.diffuse", new Vector3(0.4f, 0.4f, 0.4f));
            _lightingShader.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));
            
            // Point lights
            for (int i = 0; i < _pointLightPositions.Length; i++)
            {
                _lightingShader.SetVector3($"pointLights[{i}].position", _pointLightPositions[i]);
                _lightingShader.SetVector3($"pointLights[{i}].ambient", new Vector3(0.05f, 0.05f, 0.05f));
                _lightingShader.SetVector3($"pointLights[{i}].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
                _lightingShader.SetVector3($"pointLights[{i}].specular", new Vector3(1.0f, 1.0f, 1.0f));
                _lightingShader.SetFloat($"pointLights[{i}].constant", 1.0f);
                _lightingShader.SetFloat($"pointLights[{i}].linear", 0.09f);
                _lightingShader.SetFloat($"pointLights[{i}].quadratic", 0.032f);
            }
            
            // Spot light
            _lightingShader.SetVector3("spotLight.position", _camera.Position);
            _lightingShader.SetVector3("spotLight.direction", _camera.Front);
            _lightingShader.SetVector3("spotLight.ambient",  new Vector3(0.0f, 0.0f, 0.0f));
            _lightingShader.SetVector3("spotLight.diffuse",  new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader.SetVector3("spotLight.specular",  new Vector3(1.0f, 1.0f, 1.0f));
            _lightingShader.SetFloat("spotLight.constant", 1.0f);
            _lightingShader.SetFloat("spotLight.linear", 0.09f);
            _lightingShader.SetFloat("spotLight.quadratic", 0.032f);
            _lightingShader.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
            _lightingShader.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f))); 

            for (int i = 0; i < _cubePositions.Length; i++)
            {
                Matrix4 model = Matrix4.Identity;
                model *= Matrix4.CreateTranslation(_cubePositions[i]);
                float angle = 20.0f * i;
                model *= Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
                _lightingShader.SetMatrix4("model", model);
                
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);                
            }

            GL.BindVertexArray(_vaoModel);
            
            _lampShader.Use();

            _lampShader.SetMatrix4("view", _camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            // We use a loop to draw all the lights at the proper position
            for (int i = 0; i < _pointLightPositions.Length; i++)
            {
                Matrix4 lampMatrix = Matrix4.Identity;
                lampMatrix *= Matrix4.CreateScale(0.2f);
                lampMatrix *= Matrix4.CreateTranslation(_pointLightPositions[i]);
                
                _lampShader.SetMatrix4("model", lampMatrix);
                
                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

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