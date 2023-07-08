using ImGuiNET;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Entities;
using OpenTKEngine.Models.Shapes3D;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Models;
using static OpenTKEngine.Models.Constants;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Scenes
{
    public class BaseDebugScene : Scene
    {
        public BaseDebugScene(string name) : base(name)
        {
        }

        protected override EntityComponentManager _entityComponentManager => SceneScopedService<EntityComponentManager, BaseDebugScene>.Instance;

        public override void OnLoad()
        {
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f); // background

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);


            _shaders = new Dictionary<string, Shader>()
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

            Entity player = _entityComponentManager.AddEntity();
            player.AddComponent(new PlayerComponent(_shaders[ShaderConstants.TextureShader], new Vector3(-1.5f, -0.5f, 0.0f)));
            CameraComponent camera = player.GetComponent<CameraComponent>();

            Entity pointLight1 = _entityComponentManager.AddEntity();
            Entity pointLight2 = _entityComponentManager.AddEntity();
            Entity pointLight3 = _entityComponentManager.AddEntity();
            Entity pointLight4 = _entityComponentManager.AddEntity();
            pointLight1.AddComponent(new PointLightComponent(_shaders[ShaderConstants.TextureShader], new Vector3(0.7f, 0.2f, 2.0f)));
            pointLight2.AddComponent(new PointLightComponent(_shaders[ShaderConstants.TextureShader], new Vector3(2.3f, -3.3f, -4.0f)));
            pointLight3.AddComponent(new PointLightComponent(_shaders[ShaderConstants.TextureShader], new Vector3(-4.0f, 2.0f, -12.0f)));
            pointLight4.AddComponent(new PointLightComponent(_shaders[ShaderConstants.TextureShader], new Vector3(0.0f, 0.0f, -3.0f)));

            Entity directionalLightComponent = _entityComponentManager.AddEntity();
            directionalLightComponent.AddComponent(new DirectionalLightComponent(_shaders[ShaderConstants.TextureShader], new Vector3(-0.2f, -1.0f, -0.3f)));

            Entity materialComponent = _entityComponentManager.AddEntity();
            materialComponent.AddComponent(new MaterialComponent(_shaders[ShaderConstants.TextureShader]));

            Entity shape0 = _entityComponentManager.AddEntity();
            Entity shape1 = _entityComponentManager.AddEntity();
            Entity shape2 = _entityComponentManager.AddEntity();
            Entity shape3 = _entityComponentManager.AddEntity();
            Entity shape4 = _entityComponentManager.AddEntity();
            Entity shape5 = _entityComponentManager.AddEntity();
            Entity shape6 = _entityComponentManager.AddEntity();
            Entity shape7 = _entityComponentManager.AddEntity();
            Entity shape8 = _entityComponentManager.AddEntity();
            Entity shape9 = _entityComponentManager.AddEntity();
            shape1.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(0.0f, 0.0f, 0.0f), textures: containerTextures));
            shape2.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(2.0f, 5.0f, -15.0f), textures: containerTextures));
            shape3.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-1.5f, -2.2f, -2.5f), textures: containerTextures));
            shape4.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-3.8f, -2.0f, -12.3f), textures: containerTextures));
            shape5.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(2.4f, -0.4f, -3.5f), textures: containerTextures));
            shape6.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-1.7f, 3.0f, -7.5f), textures: containerTextures));
            shape7.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(1.3f, -2.0f, -2.5f), textures: containerTextures));
            shape8.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(1.5f, 2.0f, -2.5f), textures: containerTextures));
            shape9.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(1.5f, 0.2f, -1.5f), textures: containerTextures));
            shape0.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Cube(), new Vector3(-1.3f, 1.0f, -1.5f), textures: containerTextures));

            Entity lamp0 = _entityComponentManager.AddEntity();
            lamp0.AddComponent(new ShapeComponent(_shaders[ShaderConstants.LightShader], new Cube(), new Vector3(-5.0f, 1.0f, -1.5f)));

            Entity sphere = _entityComponentManager.AddEntity();
            sphere.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Sphere(), new Vector3(-5.0f, 3.0f, -1.5f)));

            Entity lampSphere = _entityComponentManager.AddEntity();
            lampSphere.AddComponent(new ShapeComponent(_shaders[ShaderConstants.LightShader], new Sphere(), new Vector3(-5.0f, -3.0f, -1.5f)));

            Entity plane = _entityComponentManager.AddEntity();
            plane.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Plane(5), new Vector3(5.0f, 0.0f, -1.5f)));
            plane.GetComponent<TransformComponent>().RotateTo(new AxisAngle(new Vector3(1.0f, 0.0f, 0.0f), 0.0f));

            Entity canvas = _entityComponentManager.AddEntity();
            canvas.AddComponent(new CanvasComponent(_shaders[ShaderConstants.TextShader]));
            const ImGuiWindowFlags baseFlags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoSavedSettings;
            canvas.GetComponent<CanvasComponent>().AddUIElement(new Label("Menu", baseFlags, "window0"));
            canvas.GetComponent<CanvasComponent>().AddUIElement(new Button(() => Console.WriteLine("Options"), "Options", new Vector2(250, 50), baseFlags, "menuWindow"));
            canvas.GetComponent<CanvasComponent>().AddUIElement(new Button(() => Environment.Exit(0), "Quit", new Vector2(250, 50), baseFlags, "menuWindow"));
            canvas.GetComponent<CanvasComponent>().AddUIElement(new TextBox("Chat", (string s) => Console.WriteLine(s), baseFlags, "chatWindow"));
            canvas.GetComponent<CanvasComponent>().IsVisible = false;
            canvas.GetComponent<CanvasComponent>().IsEnabled = false;
        }
    }
}
