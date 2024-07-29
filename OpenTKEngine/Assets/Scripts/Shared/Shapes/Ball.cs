using OpenTKEngine.Entities.Components;
using OpenTKEngine.Entities;
using OpenTKEngine.Models;
using static OpenTKEngine.Models.Constants;
using OpenTK.Mathematics;
using OpenTKEngine.Models.Shapes3D;

namespace OpenTKEngine.Assets.Scripts.Shared.Shapes
{
    public class Ball : Entity
    {
        private Shader _shader;
        public Vector3 _position;
        public Quaternion? _rotation;
        public Vector3? _scale;

        public Ball(Shader shader, Vector3 position, Quaternion? rotation = null, Vector3? scale = null)
        {
            _shader = shader;
            _position = position;
            _rotation = rotation;
            _scale = scale;

            Transform = new Transform(position, rotation, scale ?? new Vector3(1.0f, 1.0f, 1.0f));
            AddComponent(new ShapeComponent(_shader, new Sphere(), new List<Texture>()
            {
                Texture.LoadFromFile($"{AssetRoutes.Textures}/planegray.png")
            }));
            AddComponent(new SphereRigidBodyComponent(new BepuPhysics.Collidables.Sphere(1.0f), 5.0f));
        }
    }
}
