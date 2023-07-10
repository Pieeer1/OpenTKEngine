using OpenTKEngine.Models;
using OpenTKEngine.Models.Shapes3D.Models;
using OpenTK.Mathematics;

namespace OpenTKEngine.Entities.Components
{
    public class ModelComponent : ThreeDimensionalRenderedComponent
    {
        private readonly Model _model;
        public ModelComponent(Shader shader, Model model,Vector3 position, AxisAngle? rotation = null, Vector3? scale = null, List<Texture>? textures = null) : base(shader, position, rotation, scale, textures)
        {
            _model = model;
        }
        public ModelComponent(Shader shader, Model model, TransformComponent transform, List<Texture>? textures = null) : base(shader, transform, textures)
        {
            _model = model;
        }
        public override void BindAndBuffer()
        {
            _model.Init(_shader);
        }
       
        public override void DrawComp()
        {
            _model.Draw(_shader, _transform!);
        }
    }
}
