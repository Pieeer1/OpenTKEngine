using OpenTKEngine.Entities.Components;
using OpenTKEngine.Entities;
using OpenTKEngine.Models;
using OpenTKEngine.Services;
using static OpenTKEngine.Models.Constants;
using OpenTK.Mathematics;
using OpenTKEngine.Models.Shapes3D;

namespace OpenTKEngine.Assets.Scripts.Shared.Shapes
{
    public class Ball
    {
        private Shader _shader;
        private Entity _entity;
        public Vector3 _position;
        public Quaternion? _rotation;
        public Vector3? _scale;

        public Ball(Shader shader, Entity entity, Vector3 position, Quaternion? rotation = null, Vector3? scale = null)
        {
            _shader = shader;
            _entity = entity;
            _position = position;
            _rotation = rotation;
            _scale = scale;

            _entity.AddComponent(new ShapeComponent(_shader, new Sphere(), position, rotation, scale, new List<Texture>()
            {
                Texture.LoadFromFile($"{AssetRoutes.Textures}/planegray.png")
            }));
            _entity.AddComponent(new SphereRigidBodyComponent(new BepuPhysics.Collidables.Sphere(1.0f), 5.0f));
        }
    }
}
