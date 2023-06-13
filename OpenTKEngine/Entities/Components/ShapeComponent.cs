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
        private readonly Quaternion? _rotation;
        private readonly Vector3? _scale;
        private int _vaoModel;
        private TransformComponent _transform = null!;
        public ShapeComponent(Shader shader, Shape3D shape, Vector3 position, Quaternion? rotation = null, Vector3? scale = null) 
        {
            _shader = shader;
            _shape = shape;
            _position = position;
            _rotation = rotation;
            _scale = scale;
        }

        public override void Init()
        {
            base.Init();

            _shape.BindAndBuffer(_shader, out _vaoModel);

            _transform = Entity.AddComponent(new TransformComponent(_position, _rotation, _scale));
        }
        public override void Draw()
        {
            base.Draw();

            GL.BindVertexArray(_vaoModel);

            CameraComponent camera = EntityComponentManager.Instance.GetEntitiesWithType<CameraComponent>().FirstOrDefault()?.GetComponent<CameraComponent>() ?? throw new NullReferenceException("No Camera In Scene");

            _shader.SetMatrix4("view", camera.GetViewMatrix());
            _shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            Matrix4 model = Matrix4.CreateTranslation(_transform.Position);
            Matrix4.CreateFromQuaternion(_transform.Rotation, out Matrix4 rotationModel);
            model = model * rotationModel * Matrix4.CreateScale(_transform.Scale);
            _shader.SetMatrix4("model", model);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);

        }
        public override void Update() 
        {
            base.Update();
        }
    }
}
