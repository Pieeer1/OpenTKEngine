using OpenTKEngine.Models;
using OpenTKEngine.Models.Shapes3D;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Entities.Components
{
    public class ShapeComponent : Component
    {
        private readonly Shader _shader;
        private readonly Shape3D _shape;
        private readonly Vector3 _position;
        private readonly AxisAngle? _rotation;
        private readonly Vector3? _scale;
        private TransformComponent _transform = null!;
        private List<Texture> _textures = new List<Texture>();
        public ShapeComponent(Shader shader, Shape3D shape, Vector3 position, AxisAngle? rotation = null, Vector3? scale = null, List<Texture>? textures = null) 
        {
            _shader = shader;
            _shape = shape;
            _position = position;
            _rotation = rotation;
            _scale = scale;
            _textures = textures ?? new List<Texture>();
        }        
        public ShapeComponent(Shader shader, Shape3D shape, TransformComponent transform, List<Texture>? textures = null) 
        {
            _shader = shader;
            _shape = shape;
            _transform = transform;
            _textures = textures ?? new List<Texture>();
        }
        public override void Init()
        {
            base.Init();

            _shape.BindAndBuffer(_shader);

            _transform = Entity.AddComponent(new TransformComponent(_position, _rotation, _scale));
        }
        public override void Draw()
        {
            base.Draw();

            for (int i = 0; i < _textures.Count; i++)
            {
                _textures[i].Use(TextureUnit.Texture0 + i);
            }

            _shape.Draw(_shader, _transform);

        }
        public override void Update() 
        {
            base.Update();
        }
    }
}
