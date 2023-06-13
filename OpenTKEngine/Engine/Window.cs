using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models;
using OpenTKEngine.Models.Shapes3D;
using System;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Engine
{
    public class Window : GameWindow
    {
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

        private int _vertexBufferObject ;

        private int _vaoModel;

        private int _vaoLamp;

        private Shader _lightShader;
        private Shader _lampShader;

        private Texture _diffuseMap = null!;

        private Texture _specularMap = null!;

        private CameraComponent _camera = null!;

        private bool _firstMove = true;

        private Vector2 _lastPos;

        private readonly EntityComponentManager _entityComponentManager;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            _entityComponentManager = EntityComponentManager.Instance;
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f); // background

            GL.Enable(EnableCap.DepthTest);

            _lightShader = new Shader(ShaderRoutes.BaseVertexShader, ShaderRoutes.BaseLightingShader);
            _lampShader = new Shader(ShaderRoutes.BaseVertexShader, ShaderRoutes.BaseFragmentShader);

            Cube containers = new Cube();
            containers.BindAndBuffer(_lightShader, out _vaoModel);
            Cube lamps = new Cube();
            lamps.BindAndBuffer(_lampShader, out _vaoLamp);

            _diffuseMap = Texture.LoadFromFile($"{AssetRoutes.Textures}/container2.png");
            _specularMap = Texture.LoadFromFile($"{AssetRoutes.Textures}/container2_specular.png");

            Entity? cam = _entityComponentManager.AddEntity();
            cam.AddComponent(new CameraComponent(_lightShader, Size.X / (float)Size.Y));
            cam.AddComponent(new SpotLightComponent(_lightShader, cam.GetComponent<TransformComponent>().Position));
            _camera = cam.GetComponent<CameraComponent>();

            Entity? pointLight1 = _entityComponentManager.AddEntity();
            Entity? pointLight2 = _entityComponentManager.AddEntity();
            Entity? pointLight3 = _entityComponentManager.AddEntity();
            Entity? pointLight4 = _entityComponentManager.AddEntity();
            pointLight1.AddComponent(new PointLightComponent(_lightShader, _lampShader, new Vector3(0.7f, 0.2f, 2.0f)));
            pointLight2.AddComponent(new PointLightComponent(_lightShader, _lampShader, new Vector3(2.3f, -3.3f, -4.0f)));
            pointLight3.AddComponent(new PointLightComponent(_lightShader, _lampShader, new Vector3(-4.0f, 2.0f, -12.0f)));
            pointLight4.AddComponent(new PointLightComponent(_lightShader, _lampShader, new Vector3(0.0f, 0.0f, -3.0f)));

            Entity? directionalLightComponent = _entityComponentManager.AddEntity();
            directionalLightComponent.AddComponent(new DirectionalLightComponent(_lightShader, new Vector3(-0.2f, -1.0f, -0.3f)));

            Entity? materialComponent = _entityComponentManager.AddEntity();
            materialComponent.AddComponent(new MaterialComponent(_lightShader));

            CursorState = CursorState.Grabbed;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            GL.BindVertexArray(_vaoModel);

            _diffuseMap.Use(TextureUnit.Texture0);
            _specularMap.Use(TextureUnit.Texture1);
            _lightShader.Use();

            _entityComponentManager.Draw();


            for (int i = 0; i < _cubePositions.Length; i++)
            {
                Matrix4 model = Matrix4.CreateTranslation(_cubePositions[i]);
                float angle = 20.0f * i;
                model = model * Matrix4.CreateFromAxisAngle(new Vector3(1.0f, 0.3f, 0.5f), angle);
                _lightShader.SetMatrix4("model", model);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }

            GL.BindVertexArray(_vaoLamp);

            _lampShader.Use();

            _entityComponentManager.PostDraw();
                 
            SwapBuffers();

            _entityComponentManager.Refresh();
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            _entityComponentManager.Update();

            if (!IsFocused)
            {
                return;
            }

            var input = KeyboardState;

            _entityComponentManager.UpdateInput(e, input, MouseState, ref _firstMove, ref _lastPos);

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            _camera.Fov -= e.OffsetY;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            _camera.AspectRatio = Size.X / (float)Size.Y;
        }
    }
}
