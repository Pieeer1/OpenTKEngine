using OpenTKEngine.Models;
using OpenTK.Mathematics;
using static OpenTKEngine.Models.Constants;
using OpenTKEngine.Models.Text;
using OpenTKEngine.Attributes;
using OpenTKEngine.Models.Shapes2D;

namespace OpenTKEngine.Entities.Components
{
    public class ButtonComponent : Component
    {
        private TransformComponent _transformComponent = null!;
        private TextComponent _textComponent = null!;
        private Rectangle _rectangle = null!; // change to dynamic 2d shape
        private Texture _texture;
        [OnResize]
        public Vector2 ScreenSize { set { 
                _textComponent.ScreenSize = value; 
                _rectangle.ScreenSize = value; 
            } }

        private readonly Shader _shader;
        private readonly Vector2 _location;
        private readonly string _text;
        private readonly float _scale;
        private readonly Vector3 _textColor;


        public ButtonComponent(Shader shader, Vector2 location, string text, Texture texture, float scale = 1.0f, Vector3? textColor = null)
        {
            _shader = shader;
            _location = location;
            _text = text;
            _texture = texture;
            _scale = scale;
            _textColor = textColor ?? ColorVectors.White;
        }

        public override void Init()
        {
            base.Init();

            _transformComponent = Entity.AddComponent(new TransformComponent(new Vector3(_location.X, _location.Y, 0.0f)));

            float width = 400.0f;
            float height = 125.0f;

            _rectangle = new Rectangle(new Vector2(_transformComponent.Position.X, _transformComponent.Position.Y), width, height, ColorVectors.Black, _texture);

            _textComponent = Entity.AddComponent(new TextComponent(_shader, _text, new Vector2(_transformComponent.Position.X - width/4, _transformComponent.Position.Y), _scale, _textColor), 5);

            _rectangle.BindAndBuffer(_shader);
        }
        public override void Draw()
        {
            base.Draw();
            _rectangle.Draw(_shader, _transformComponent);
        }

    }
}
