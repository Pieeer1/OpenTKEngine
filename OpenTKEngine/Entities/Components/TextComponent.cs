using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Attributes;
using OpenTKEngine.Models;
using OpenTKEngine.Models.Text;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Entities.Components
{
    public class TextComponent : Component
    {
        private TransformComponent _transformComponent = null!;
        private readonly Shader _shader;
        private string _displayText { get; set; }
        private Vector2 _position { get; set; }
        private float _scale { get; set; }
        private Vector3 _color { get; set; }
        private RenderedString _renderedString = null!;
        [OnResize]
        public Vector2 ScreenSize { set => _renderedString.ScreenSize = value; }

        public TextComponent(Shader shader, string displayText, Vector2 position, float scale = 1.0f, Vector3? color = null ) 
        {
            _shader = shader;
            _displayText = displayText;
            _position = position;
            _scale = scale;
            _color = color ?? ColorVectors.White;
        }

        public override void Init()
        {
            base.Init();
            _transformComponent = Entity.AddComponent(new TransformComponent(new Vector3(_position.X, _position.Y, 0.0f)));

            _renderedString = new RenderedString(_displayText, _position.X, _position.Y, _scale, _color);
            _renderedString.BindAndBuffer(_shader);
        }
        public override void Draw()
        {
            base.Draw();
            _renderedString.Draw(_shader, _transformComponent);
        }
    }
}
