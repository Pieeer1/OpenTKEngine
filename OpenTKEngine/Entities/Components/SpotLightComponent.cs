using OpenTK.Mathematics;
using OpenTKEngine.Models;
using OpenTKEngine.Scenes;

namespace OpenTKEngine.Entities.Components
{
    public class SpotLightComponent : Component
    {
        private TransformComponent _transform = null!;
        private readonly Shader _shader;
        private readonly Vector3 _position;
        private readonly Vector3 _ambient;
        private readonly Vector3 _diffuse;
        private readonly Vector3 _specular;
        private readonly float _constant;
        private readonly float _linear;
        private readonly float _quadratic;
        private readonly float _cutOff;
        private readonly float _outerCutOff;
        private bool _isActive = true;
        public SpotLightComponent(Shader shader, Vector3 position, Vector3? ambient = null, Vector3? diffuse = null, Vector3? specular = null, float? constant = null, float? linear = null, float? quadratic = null, float? cutOff = null, float? outerCutOff = null)
        {
            _shader = shader;
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

            _transform = Entity.AddComponent(new TransformComponent(_position));
        }
        public override void Draw()
        {
            base.Draw();

            CameraComponent camera = SceneManager.Instance.ActiveScene.EntityComponentManager.GetEntitiesWithType<CameraComponent>().FirstOrDefault()?.GetComponent<CameraComponent>() ?? throw new NullReferenceException("No Camera In Scene");

            _shader.SetVector3("spotLight.position", camera.Transform.Position);
            _shader.SetVector3("spotLight.direction", camera.Front);
            _shader.SetVector3("spotLight.ambient", _ambient);
            _shader.SetVector3("spotLight.diffuse", _diffuse);
            _shader.SetVector3("spotLight.specular", _specular);
            _shader.SetFloat("spotLight.constant", _constant);
            _shader.SetFloat("spotLight.linear", _linear);
            _shader.SetFloat("spotLight.quadratic", _quadratic);
            _shader.SetFloat("spotLight.cutOff", _isActive ? _cutOff : float.MaxValue);
            _shader.SetFloat("spotLight.outerCutOff", _isActive? _outerCutOff : float.MaxValue);
            
        }
        public override void Update()
        {
            base.Update();
        }

        public void ToggleLight()
        {
            _isActive = !_isActive;
        }
    }
}
