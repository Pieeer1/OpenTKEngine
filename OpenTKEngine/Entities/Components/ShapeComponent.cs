using OpenTKEngine.Models;
using OpenTKEngine.Models.Shapes3D;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

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
        public ShapeComponent(Shader shader, Shape3D shape, Vector3 position, AxisAngle? rotation = null, Vector3? scale = null) 
        {
            _shader = shader;
            _shape = shape;
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }        
        public ShapeComponent(Shader shader, Shape3D shape, TransformComponent transform) 
        {
            _shader = shader;
            _shape = shape;
            _transform = transform;
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

            _shape.Draw(_shader, _transform);

        }
        public override void Update() 
        {
            base.Update();
        }
    }
}
