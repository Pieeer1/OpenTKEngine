using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTKEngine.Assets.Scripts.Shared.UI;
using OpenTKEngine.Entities;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models;
using OpenTKEngine.Services;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Scenes
{
    public class MainMenuScene : Scene
    {

        public MainMenuScene(string name) : base(name)
        {
        }

        public override void OnAwake()
        {
            Vector4 backgroundColor = Styles.ColorDictionary["BackgroundColor"];
            GL.ClearColor(backgroundColor.X, backgroundColor.Y, backgroundColor.Z, backgroundColor.W); // background

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            WindowService.Instance.ActiveCursorState = CursorState.Normal;

            _shaders = new Dictionary<string, Shader>()
            {
                { ShaderConstants.TextShader, new Shader(ShaderRoutes.BaseTextVertexShader, ShaderRoutes.BaseTextFragmentShader) }
            };


            Entity canvas = EntityComponentManager.AddEntity();
            CanvasComponent canvasComp = canvas.AddComponent(new CanvasComponent(_shaders[ShaderConstants.TextShader]));
            MainMenu menu = new MainMenu(canvasComp);


        }
    }
}
