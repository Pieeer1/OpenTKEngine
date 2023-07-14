using OpenTKEngine.Entities;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;
using static OpenTKEngine.Models.Constants;
using OpenTKEngine.Models;
using OpenTKEngine.Services;

namespace OpenTKEngine.Assets.Scripts.Shared.Shapes
{
    public class Plane
    {
        private Shader _shader;
        private Entity _entity;
        public Vector3 _position;
        public Quaternion? _rotation;
        public Vector2? _scale;

        public Plane(Shader shader, Entity entity, Vector3 position, Quaternion? rotation = null, Vector2? scale = null)
        {
            _shader = shader;
            _entity = entity;
            _position = position;
            _rotation = rotation;
            _scale = scale;

            _entity.AddComponent(new ModelComponent(_shader, new Models.Shapes3D.Models.Model($"{AssetRoutes.Models}/plane.dae"), position, rotation, scale != null ? new Vector3(scale.Value.X, 0.01f, scale.Value.Y) : new Vector3(1.0f, 0.01f, 1.0f), new List<Texture>()
            {
                Texture.LoadFromFile($"{AssetRoutes.Textures}/planegray.png")
            }));
            _entity.AddComponent(new StaticRigidBodyComponent());
        }
    }
}
