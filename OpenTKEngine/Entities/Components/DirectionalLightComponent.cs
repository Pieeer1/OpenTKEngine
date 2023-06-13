using OpenTK.Mathematics;
using OpenTKEngine.Models;


namespace OpenTKEngine.Entities.Components
{
    public class DirectionalLightComponent : Component
    {
        private readonly Shader _lightShader;
        private readonly Vector3 _direction;
        private readonly Vector3 _ambient;
        private readonly Vector3 _diffuse;
        private readonly Vector3 _specular;

        public DirectionalLightComponent(Shader lightShader, Vector3 direction, Vector3? ambient = null, Vector3? diffuse = null, Vector3? specular = null)
        {
            _lightShader = lightShader;
            _direction = direction;
            _ambient = ambient ?? new Vector3(0.05f, 0.05f, 0.05f);
            _diffuse = diffuse ?? new Vector3(0.4f, 0.4f, 0.4f);
            _specular = specular ?? new Vector3(0.5f, 0.5f, 0.5f);
        }

        public override void Draw()
        {
            _lightShader.SetVector3("dirLight.direction", _direction);
            _lightShader.SetVector3("dirLight.ambient", _ambient);
            _lightShader.SetVector3("dirLight.diffuse", _diffuse);
            _lightShader.SetVector3("dirLight.specular", _specular);
        }
        public override void Init()
        {
            base.Init();
        }
        public override void Update()
        {
            base.Update();
        }
    }
}
