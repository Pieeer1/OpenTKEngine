using OpenTK.Mathematics;
using OpenTKEngine.Models;

namespace OpenTKEngine.Entities.Components
{
    public class SpotLightComponent : Component
    {
        private readonly Shader _lightShader;
        private TransformComponent _transform = null!;

        private readonly Vector3 _position;
        private readonly Vector3 _ambient;
        private readonly Vector3 _diffuse;
        private readonly Vector3 _specular;
        private readonly float _constant;
        private readonly float _linear;
        private readonly float _quadratic;
        private readonly float _cutOff;
        private readonly float _outerCutOff;
        public SpotLightComponent(Shader lightShader, Vector3 position, Vector3? ambient = null, Vector3? diffuse = null, Vector3? specular = null, float? constant = null, float? linear = null, float? quadratic = null, float? cutOff = null, float? outerCutOff = null)
        {
            _lightShader = lightShader;
            _position = position;
            _ambient = ambient ?? new Vector3(0.0f, 0.0f, 0.0f);
            _diffuse = diffuse ?? new Vector3(1.0f, 1.0f, 1.0f);
            _specular = specular ?? new Vector3(1.0f, 1.0f, 1.0f);
            _constant = constant ?? 1.0f;
            _linear = linear ?? 0.09f;
            _quadratic = quadratic ?? 0.032f;
            _cutOff = cutOff ?? MathF.Cos(MathHelper.DegreesToRadians(12.5f));
            _outerCutOff = outerCutOff ?? MathF.Cos(MathHelper.DegreesToRadians(17.5f));
        }

        public override void Init()
        {
            base.Init();

            Entity.AddComponent<TransformComponent>(new TransformComponent(_position));
            _transform = Entity.GetComponent<TransformComponent>();
        }
        public override void Draw()
        {
            base.Draw();

            CameraComponent camera = EntityComponentManager.Instance.GetEntitiesWithType<CameraComponent>().FirstOrDefault()?.GetComponent<CameraComponent>() ?? throw new NullReferenceException("No Camera In Scene");

            _lightShader.SetVector3("spotLight.position", camera.Transform.Position);
            _lightShader.SetVector3("spotLight.direction", camera.Front);
            _lightShader.SetVector3("spotLight.ambient", _ambient);
            _lightShader.SetVector3("spotLight.diffuse", _diffuse);
            _lightShader.SetVector3("spotLight.specular", _specular);
            _lightShader.SetFloat("spotLight.constant", _constant);
            _lightShader.SetFloat("spotLight.linear", _linear);
            _lightShader.SetFloat("spotLight.quadratic", _quadratic);
            _lightShader.SetFloat("spotLight.cutOff", _cutOff);
            _lightShader.SetFloat("spotLight.outerCutOff", _outerCutOff);

        }
        public override void Update()
        {
            base.Update();
        }

    }
}
