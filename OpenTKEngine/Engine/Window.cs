using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Attributes;
using OpenTKEngine.Entities;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models;
using OpenTKEngine.Models.Shapes3D;
using OpenTKEngine.Models.UI;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Engine
{
    public class Window : GameWindow
    {

        private Dictionary<string, Shader> Shaders = null!;

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

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f); // background

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);


            Shaders = new Dictionary<string, Shader>()
            {
                { ShaderConstants.TextureShader, new Shader(ShaderRoutes.BaseVertexShader, ShaderRoutes.BaseLightingShader)},
                { ShaderConstants.LightShader, new Shader(ShaderRoutes.BaseVertexShader, ShaderRoutes.BaseFragmentShader)},
                { ShaderConstants.TextShader, new Shader(ShaderRoutes.BaseTextVertexShader, ShaderRoutes.BaseTextFragmentShader)}
            };

            List<Texture> containerTextures = new List<Texture>
            {
                Texture.LoadFromFile($"{AssetRoutes.Textures}/container2.png"),
                Texture.LoadFromFile($"{AssetRoutes.Textures}/container2_specular.png"),
            };

            Texture blankCanvas = Texture.LoadFromFile($"{AssetRoutes.Textures}/blank_canvas.png");

            Entity ? player = _entityComponentManager.AddEntity();
            player.AddComponent(new PlayerComponent(Shaders[ShaderConstants.TextureShader], new Vector3(-1.5f, -0.5f, 0.0f)));
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
            shape1.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(0.0f, 0.0f, 0.0f), textures: containerTextures));
            shape2.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(2.0f, 5.0f, -15.0f), textures: containerTextures));
            shape3.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-1.5f, -2.2f, -2.5f), textures: containerTextures));
            shape4.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-3.8f, -2.0f, -12.3f), textures: containerTextures));
            shape5.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(2.4f, -0.4f, -3.5f), textures: containerTextures));
            shape6.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-1.7f, 3.0f, -7.5f), textures: containerTextures));
            shape7.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(1.3f, -2.0f, -2.5f), textures: containerTextures));
            shape8.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(1.5f, 2.0f, -2.5f), textures: containerTextures));
            shape9.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(1.5f, 0.2f, -1.5f), textures: containerTextures));
            shape0.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-1.3f, 1.0f, -1.5f), textures: containerTextures));

            Entity? lamp0 = _entityComponentManager.AddEntity();
            lamp0.AddComponent(new ShapeComponent(Shaders[ShaderConstants.LightShader], new Cube(), new Vector3(-5.0f, 1.0f, -1.5f)));

            Entity? sphere = _entityComponentManager.AddEntity();
            sphere.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Sphere(), new Vector3(-5.0f, 3.0f, -1.5f)));

            Entity? lampSphere = _entityComponentManager.AddEntity();
            lampSphere.AddComponent(new ShapeComponent(Shaders[ShaderConstants.LightShader], new Sphere(), new Vector3(-5.0f, -3.0f, -1.5f)));

            Entity? plane = _entityComponentManager.AddEntity();
            plane.AddComponent(new ShapeComponent(Shaders[ShaderConstants.TextureShader], new Plane(5), new Vector3(5.0f, 0.0f, -1.5f)));
            plane.GetComponent<TransformComponent>().RotateTo(new AxisAngle(new Vector3(1.0f, 0.0f, 0.0f), 0.0f));

            Entity? canvas = _entityComponentManager.AddEntity();
            canvas.AddComponent(new CanvasComponent(Shaders[ShaderConstants.TextShader]));
            canvas.GetComponent<CanvasComponent>().AddUIElement(new Button(() => Console.WriteLine("Hello World"), "test button", new Vector2(250, 50), ImGuiNET.ImGuiWindowFlags.NoResize | ImGuiNET.ImGuiWindowFlags.NoTitleBar | ImGuiNET.ImGuiWindowFlags.NoSavedSettings, "button1"));

            CursorState = CursorState.Grabbed;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            _entityComponentManager.Draw();
                 
            SwapBuffers();

            _entityComponentManager.Refresh();
        }

        private int Tick;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Tick++;

            _entityComponentManager.Update();

            if (!IsFocused)
            {
                return;
            }

            var input = KeyboardState;

            _entityComponentManager.UpdateInput(e, input, MouseState, ref _firstMove, ref _lastPos);
            _entityComponentManager.SetComponentReferencesWithAttribute(new OnTickAttribute(), Tick);


            if (input.IsKeyPressed(Keys.Escape))
            { 
                CursorState = CursorState == CursorState.Grabbed ? CursorState.Normal : CursorState.Grabbed;
            }
            if (input.IsKeyDown(Keys.Escape) && input.IsKeyDown(Keys.LeftShift))
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

            HandleResizeAttributes(Size);
        }
        private void HandleResizeAttributes(Vector2 screenSize)
        {
            _entityComponentManager.SetComponentReferencesWithAttribute(new OnResizeAttribute(), screenSize);
        }
    }
}
