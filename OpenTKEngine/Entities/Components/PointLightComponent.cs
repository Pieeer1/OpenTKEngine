using OpenTKEngine.Models;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace OpenTKEngine.Entities.Components
{
    public class PointLightComponent : Component
    {
        private TransformComponent _transform = null!;
        private readonly Vector3 _position;
        private readonly Vector3 _ambient;
        private readonly Vector3 _diffuse;
        private readonly Vector3 _specular;
        private Shader _lightShader;
        private Shader _lampShader;
        private readonly float _constant;
        private readonly float _linear;
        private readonly float _quadratic;
        public PointLightComponent(Shader lightShader, Shader lampShader, Vector3 position, Vector3? ambient = null, Vector3? diffuse = null, Vector3? specular = null, float? constant = null, float? linear = null, float? quadratic = null)
        {
            _lightShader = lightShader;
            _lampShader = lampShader;
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

            Entity.AddComponent<TransformComponent>(new TransformComponent(_position));
            _transform = Entity.GetComponent<TransformComponent>();
        }
        public override void Draw()
        {
            base.Draw();

            var entities = EntityComponentManager.Instance.GetEntitiesWithType<PointLightComponent>();
            int index = entities.ToList().IndexOf(Entity);

            _lightShader.SetVector3($"pointLights[{index}].position", _position);
            _lightShader.SetVector3($"pointLights[{index}].ambient", _ambient);
            _lightShader.SetVector3($"pointLights[{index}].diffuse", _diffuse);
            _lightShader.SetVector3($"pointLights[{index}].specular", _specular);
            _lightShader.SetFloat($"pointLights[{index}].constant", _constant);
            _lightShader.SetFloat($"pointLights[{index}].linear", _linear);
            _lightShader.SetFloat($"pointLights[{index}].quadratic", _quadratic);

        }
        public override void PostDraw()
        {
            base.PostDraw();

            CameraComponent camera = EntityComponentManager.Instance.GetEntitiesWithType<CameraComponent>().FirstOrDefault()?.GetComponent<CameraComponent>() ?? throw new NullReferenceException("No Camera In Scene");

            _lampShader.SetMatrix4("view", camera.GetViewMatrix());
            _lampShader.SetMatrix4("projection", camera.GetProjectionMatrix());

            Matrix4 lampMatrix = Matrix4.CreateScale(0.2f);
            lampMatrix = lampMatrix * Matrix4.CreateTranslation(_position);

            _lampShader.SetMatrix4("model", lampMatrix);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
        }
        public override void Update() 
        {
            base.Update();
        }
    }
}
