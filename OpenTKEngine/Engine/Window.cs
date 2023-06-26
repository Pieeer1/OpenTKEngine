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

        private Dictionary<string, Shader> Shaders = null!;

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

            GL.ClearColor(0.05f, 0.05f, 0.05f, 1.0f); // background

            GL.Enable(EnableCap.DepthTest);

            Shaders = new Dictionary<string, Shader>()
            {
                { ShaderConstants.TextureShader, new Shader(ShaderRoutes.BaseVertexShader, ShaderRoutes.BaseLightingShader)},
                { ShaderConstants.LightShader, new Shader(ShaderRoutes.BaseVertexShader, ShaderRoutes.BaseFragmentShader)},
            };

            _diffuseMap = Texture.LoadFromFile($"{AssetRoutes.Textures}/container2.png");
            _specularMap = Texture.LoadFromFile($"{AssetRoutes.Textures}/container2_specular.png");

            Entity? player = _entityComponentManager.AddEntity();
            player.AddComponent(new PlayerComponent(Shaders[ShaderConstants.TextureShader], Size.X / (float)Size.Y));
            _camera = player.GetComponent<CameraComponent>();

            Entity? pointLight1 = _entityComponentManager.AddEntity();
            Entity? pointLight2 = _entityComponentManager.AddEntity();
            Entity? pointLight3 = _entityComponentManager.AddEntity();
            Entity? pointLight4 = _entityComponentManager.AddEntity();
            pointLight1.AddComponent(new PointLightComponent(Shaders[ShaderConstants.TextureShader], new Vector3(0.7f, 0.2f, 2.0f)));
            pointLight2.AddComponent(new PointLightComponent(Shaders[ShaderConstants.TextureShader], new Vector3(2.3f, -3.3f, -4.0f)));
            pointLight3.AddComponent(new PointLightComponent(Shaders[ShaderConstants.TextureShader], new Vector3(-4.0f, 2.0f, -12.0f)));
            pointLight4.AddComponent(new PointLightComponent(Shaders[ShaderConstants.TextureShader], new Vector3(0.0f, 0.0f, -3.0f)));

            Entity? directionalLightComponent = _entityComponentManager.AddEntity();
            directionalLightComponent.AddComponent(new DirectionalLightComponent(Shaders[ShaderConstants.TextureShader], new Vector3(-0.2f, -1.0f, -0.3f)));

            Entity? materialComponent = _entityComponentManager.AddEntity();
            materialComponent.AddComponent(new MaterialComponent(Shaders[ShaderConstants.TextureShader]));

            Entity? shape0 = _entityComponentManager.AddEntity();
            Entity? shape1 = _entityComponentManager.AddEntity();
            Entity? shape2 = _entityComponentManager.AddEntity();
            Entity? shape3 = _entityComponentManager.AddEntity();
            Entity? shape4 = _entityComponentManager.AddEntity();
            Entity? shape5 = _entityComponentManager.AddEntity();
            Entity? shape6 = _entityComponentManager.AddEntity();
            Entity? shape7 = _entityComponentManager.AddEntity();
            Entity? shape8 = _entityComponentManager.AddEntity();
            Entity? shape9 = _entityComponentManager.AddEntity();
            shape1.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(0.0f, 0.0f, 0.0f)));
            shape2.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(2.0f, 5.0f, -15.0f)));
            shape3.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-1.5f, -2.2f, -2.5f)));
            shape4.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-3.8f, -2.0f, -12.3f)));
            shape5.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(2.4f, -0.4f, -3.5f)));
            shape6.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-1.7f, 3.0f, -7.5f)));
            shape7.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(1.3f, -2.0f, -2.5f)));
            shape8.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(1.5f, 2.0f, -2.5f)));
            shape9.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(1.5f, 0.2f, -1.5f)));
            shape0.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-1.3f, 1.0f, -1.5f)));

            Entity? lamp0 = _entityComponentManager.AddEntity();
            lamp0.AddComponent(new ShapeComponent(Shaders[ShaderConstants.LightShader], new Cube(), new Vector3(-5.0f, 1.0f, -1.5f)));
                        
            Entity? sphere = _entityComponentManager.AddEntity();
            sphere.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Sphere(), new Vector3(-5.0f, 3.0f, -1.5f)));

            Entity? lampSphere = _entityComponentManager.AddEntity();
            lampSphere.AddComponent(new ShapeComponent(Shaders[ShaderConstants.LightShader], new Sphere(), new Vector3(-5.0f, -3.0f, -1.5f)));

            Entity? plane = _entityComponentManager.AddEntity();
            plane.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Plane(2), new Vector3(5.0f, 0.0f, -1.5f)));
            plane.GetComponent<TransformComponent>().RotateTo(new AxisAngle(new Vector3(1.0f, 0.0f, 0.0f), 0.0f));

             
            CursorState = CursorState.Grabbed;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _diffuseMap.Use(TextureUnit.Texture0);
            _specularMap.Use(TextureUnit.Texture1);

            foreach (Shader shader in Shaders.Values)
            {
                shader.Use();
            }

            _entityComponentManager.Draw();
                 
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
