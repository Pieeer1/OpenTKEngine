using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Attributes;
using OpenTKEngine.Models;
using OpenTKEngine.Models.UI;

namespace OpenTKEngine.Entities.Components
{
    public class CanvasComponent : Component
    {
        private Shader _shader;
        private Canvas _canvas;
        private TransformComponent _transform = null!;
        private bool _isVisible = true;
        [MenuEnable]
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }
        private bool _isEnabled = true;
        [MenuEnable]
        public bool IsEnabled { get => _isEnabled; set => _isEnabled = value; }
        public CanvasComponent(Shader shader)
        {
            _shader = shader;
            _canvas = new Canvas(shader);
        }
        [OnTick]
        public double Tick { set => _canvas.ElapsedTime = value; }
        [OnResize]
        public Vector2 ScreenSize { set => _canvas.ScreenSize = value; }

        public override void Init()
        {
            _transform = Entity.AddComponent(new TransformComponent());
        }
        public override void Draw()
        {
            if (!IsVisible) { return; }
            _canvas.Draw(_shader, _transform);
        }
        public override void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            if (!IsEnabled) { return; }
            _canvas.HandleInput(mouse);
        }
        public void AddUIElement(UIElement element)
        { 
            _canvas.UIElements.Add(element);
        }
    }
}
