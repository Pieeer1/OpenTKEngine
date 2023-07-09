using OpenTKEngine.Entities.Components;
using OpenTKEngine.Entities;
using OpenTKEngine.Models.Shapes3D;
using OpenTKEngine.Models;
using static OpenTKEngine.Models.Constants;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Models.Shared.UI;
using OpenTKEngine.Services;

namespace OpenTKEngine.Scenes
{
    public class BaseDebugScene : Scene
    {
        public BaseDebugScene(string name) : base(name)
        {
        }

        public override void OnAwake()
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

            Entity player = EntityComponentManager.AddEntity();
            player.AddComponent(new PlayerComponent(_shaders[ShaderConstants.TextureShader], new Vector3(-1.5f, -0.5f, 0.0f)));
            CameraComponent camera = player.GetComponent<CameraComponent>();

            Entity pointLight1 = EntityComponentManager.AddEntity();
            Entity pointLight2 = EntityComponentManager.AddEntity();
            Entity pointLight3 = EntityComponentManager.AddEntity();
            Entity pointLight4 = EntityComponentManager.AddEntity();
            pointLight1.AddComponent(new PointLightComponent(_shaders[ShaderConstants.TextureShader], new Vector3(0.7f, 0.2f, 2.0f)));
            pointLight2.AddComponent(new PointLightComponent(_shaders[ShaderConstants.TextureShader], new Vector3(2.3f, -3.3f, -4.0f)));
            pointLight3.AddComponent(new PointLightComponent(_shaders[ShaderConstants.TextureShader], new Vector3(-4.0f, 2.0f, -12.0f)));
            pointLight4.AddComponent(new PointLightComponent(_shaders[ShaderConstants.TextureShader], new Vector3(0.0f, 0.0f, -3.0f)));

            Entity directionalLightComponent = EntityComponentManager.AddEntity();
            directionalLightComponent.AddComponent(new DirectionalLightComponent(_shaders[ShaderConstants.TextureShader], new Vector3(-0.2f, -1.0f, -0.3f)));

            Entity materialComponent = EntityComponentManager.AddEntity();
            materialComponent.AddComponent(new MaterialComponent(_shaders[ShaderConstants.TextureShader]));

            Entity shape0 = EntityComponentManager.AddEntity();
            Entity shape1 = EntityComponentManager.AddEntity();
            Entity shape2 = EntityComponentManager.AddEntity();
            Entity shape3 = EntityComponentManager.AddEntity();
            Entity shape4 = EntityComponentManager.AddEntity();
            Entity shape5 = EntityComponentManager.AddEntity();
            Entity shape6 = EntityComponentManager.AddEntity();
            Entity shape7 = EntityComponentManager.AddEntity();
            Entity shape8 = EntityComponentManager.AddEntity();
            Entity shape9 = EntityComponentManager.AddEntity();
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

            Entity lamp0 = EntityComponentManager.AddEntity();
            lamp0.AddComponent(new ShapeComponent(_shaders[ShaderConstants.LightShader], new Cube(), new Vector3(-5.0f, 1.0f, -1.5f)));

            Entity sphere = EntityComponentManager.AddEntity();
            sphere.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Sphere(), new Vector3(-5.0f, 3.0f, -1.5f)));

            Entity lampSphere = EntityComponentManager.AddEntity();
            lampSphere.AddComponent(new ShapeComponent(_shaders[ShaderConstants.LightShader], new Sphere(), new Vector3(-5.0f, -3.0f, -1.5f)));

            Entity plane = EntityComponentManager.AddEntity();
            plane.AddComponent(new ShapeComponent(_shaders[ShaderConstants.TextureShader], new Plane(5), new Vector3(5.0f, 0.0f, -1.5f)));
            plane.GetComponent<TransformComponent>().RotateTo(new AxisAngle(new Vector3(1.0f, 0.0f, 0.0f), 0.0f));

            Entity canvas = EntityComponentManager.AddEntity();
            canvas.AddComponent(new CanvasComponent(_shaders[ShaderConstants.TextShader]));
            CanvasComponent canvasComp = canvas.GetComponent<CanvasComponent>();
            Menu menu = new Menu(canvasComp);
            ChatInput chat = new ChatInput(canvasComp);
            ChatBox chatBox = new ChatBox(canvasComp);
            MessageService.Instance.LogInformation("some information");
            MessageService.Instance.LogWarning("warning");
            MessageService.Instance.LogError("error");
            //canvasComp.IsEnabled = false;
            //canvasComp.IsVisible = false;
        }
    }
}
