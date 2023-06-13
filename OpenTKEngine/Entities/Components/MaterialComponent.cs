using OpenTKEngine.Models;
using OpenTK.Mathematics;

namespace OpenTKEngine.Entities.Components
{
    public class MaterialComponent : Component
    {
        private readonly int _diffuse;
        private readonly int _specular;
        private readonly float _shininess;
        private Shader _shader;
        public MaterialComponent(Shader shader, int? diffuse = null, int? specular = null, float? shininess = null)
        {
            _shader = shader;
            _diffuse = diffuse ?? 0;
            _specular = specular ?? 1;
            _shininess = shininess ?? 32.0f;
        }

        public override void Draw()
        {
            base.Draw();

            _shader.SetInt("material.diffuse", _diffuse);
            _shader.SetInt("material.specular", _specular);
            _shader.SetFloat("material.shininess", _shininess);
        }
    }
}
