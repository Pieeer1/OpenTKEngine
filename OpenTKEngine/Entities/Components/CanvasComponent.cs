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
        public bool IsVisible { get => _isVisible; set => _isVisible = value; }
        private bool _isEnabled = true;
        public bool IsEnabled { get => _isEnabled; set => _isEnabled = value; }
        [CharacterPress]
        public char CharacterPressed { set => _canvas.CharPressed(value); }
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
            base.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
            if (!IsEnabled) { return; }
            foreach (UIElement uiElement in _canvas.UIElements.Where(x => x.ToggleKey != Keys.Unknown))
            {
                if (input.IsKeyPressed(uiElement.ToggleKey))
                {
                    uiElement.Toggle();
                }
            }
            _canvas.HandleInput(mouse, input);
        }
        public void AddUIElement(UIElement element)
        {
            _canvas.UIElements.Add(element);
        }
        public void RemoveUIElement(UIElement element)
        {
            _canvas.UIElements.Remove(element);
        }
    }
}
