using OpenTKEngine.Models;
using OpenTKEngine.Models.Shapes3D;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace OpenTKEngine.Entities.Components
{
    public class ShapeComponent : ThreeDimensionalRenderedComponent
    {
        private readonly Shape3D _shape;

        public ShapeComponent(Shader shader, Shape3D shape, Vector3 position, Quaternion? rotation = null, Vector3? scale = null, List<Texture>? textures = null) : base(shader, position, rotation, scale, textures)
        {
            _shape = shape;
        }        
        public ShapeComponent(Shader shader, Shape3D shape, TransformComponent transform, List<Texture>? textures = null) : base(shader, transform, textures) 
        {
            _shape = shape;
        }

        public override void BindAndBuffer()
        {
            _shape.BindAndBuffer(_shader);
        }

        public override void DrawComp()
        {
            _shape.Draw(_shader, _transform!);
        }
    }
}
