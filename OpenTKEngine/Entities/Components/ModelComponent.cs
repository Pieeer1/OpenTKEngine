using OpenTKEngine.Models;
using OpenTKEngine.Models.Shapes3D.Models;
using OpenTK.Mathematics;

namespace OpenTKEngine.Entities.Components
{
    public class ModelComponent : ThreeDimensionalRenderedComponent
    {
        private readonly Model _model;
        public ModelComponent(Shader shader, Model model, List<Texture>? textures = null) : base(shader, textures)
        {
            _model = model;
        }
        public override void BindAndBuffer()
        {
            _model.Init(_shader);
        }
       
        public override void DrawComp()
        {
            _model.Draw(_shader, Entity.Transform);
        }
    }
}
