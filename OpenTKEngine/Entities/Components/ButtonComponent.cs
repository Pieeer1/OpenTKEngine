using OpenTKEngine.Models;
using OpenTK.Mathematics;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Entities.Components
{
    public class ButtonComponent : Component
    {
        private TransformComponent _transformComponent = null!;
        private TextComponent _textComponent = null!;

        private readonly Shader _shader;
        private readonly Vector2 _location;
        private readonly string _text;
        private readonly float _scale;
        private readonly Vector3 _textColor;

        public ButtonComponent(Shader shader, Vector2 location, string text, float scale = 1.0f, Vector3? textColor = null)
        {
            _shader = shader;
            _location = location;
            _text = text;
            _scale = scale;
            _textColor = textColor ?? ColorVectors.White;
        }

        public override void Init()
        {
            base.Init();

            _transformComponent = Entity.AddComponent(new TransformComponent(new Vector3(_location.X, _location.Y, 0.0f)));
            _textComponent = Entity.AddComponent(new TextComponent(_shader, _text, new Vector2(_transformComponent.Position.X, _transformComponent.Position.Y)), 5);

        }
        public override void Draw()
        {
            base.Draw();


        }

    }
}
