using OpenTKEngine.Models;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using OpenTKEngine.Scenes;

namespace OpenTKEngine.Entities.Components
{
    public class PointLightComponent : Component
    {
        private TransformComponent _transform = null!;
        private readonly Vector3 _position;
        private readonly Vector3 _ambient;
        private readonly Vector3 _diffuse;
        private readonly Vector3 _specular;
        private Shader _shader;
        private readonly float _constant;
        private readonly float _linear;
        private readonly float _quadratic;
        public PointLightComponent(Shader shader, Vector3 position, Vector3? ambient = null, Vector3? diffuse = null, Vector3? specular = null, float? constant = null, float? linear = null, float? quadratic = null)
        {
            _shader = shader;
            _position = position;
            _ambient = ambient ?? new Vector3(0.05f, 0.05f, 0.05f);
            _diffuse = diffuse ?? new Vector3(0.8f, 0.8f, 0.8f);
            _specular = specular ?? new Vector3(1.0f, 1.0f, 1.0f);
            _constant = constant ?? 1.0f;
            _linear = linear ?? 0.09f;
            _quadratic = quadratic ?? 0.032f;
        }
        public override void Init()
        {
            base.Init();

            _transform = Entity.AddComponent(new TransformComponent(_position));
        }
        public override void Draw()
        {
            base.Draw();

            var entities = EntityComponentManager.GetEntitiesWithType<PointLightComponent>();
            int index = entities.ToList().IndexOf(Entity);

            _shader.SetVector3($"pointLights[{index}].position", _position);
            _shader.SetVector3($"pointLights[{index}].ambient", _ambient);
            _shader.SetVector3($"pointLights[{index}].diffuse", _diffuse);
            _shader.SetVector3($"pointLights[{index}].specular", _specular);
            _shader.SetFloat($"pointLights[{index}].constant", _constant);
            _shader.SetFloat($"pointLights[{index}].linear", _linear);
            _shader.SetFloat($"pointLights[{index}].quadratic", _quadratic);

        }
        public override void Update() 
        {
            base.Update();
        }
    }
}
