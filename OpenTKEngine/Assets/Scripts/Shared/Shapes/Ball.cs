using BulletSharp;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Entities;
using OpenTKEngine.Models;
using OpenTKEngine.Services;
using static OpenTKEngine.Models.Constants;
using OpenTK.Mathematics;
using OpenTKEngine.Models.Shapes3D;
using OpenTKEngine.Models.Physics;

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

            //Vector3 initialOrientation = new Vector3(0.0f, 0.0f, 1.0f); // default orientation
            //Vector3 targetOrientation = new Vector3(0.0f, 1.0f, 0.0f);

            //Quaternion rotationQuaternion = Quaternion.FromAxisAngle(Vector3.Cross(initialOrientation, targetOrientation), MathHelper.DegreesToRadians(90f));

            _entity.AddComponent(new ShapeComponent(_shader, new Sphere(), position, rotation, scale, new List<Texture>()
            {
                Texture.LoadFromFile($"{AssetRoutes.Textures}/planegray.png")
            }));

            CollisionShape sphereshape = new SphereShape(1.0f); 

            float mass = 3.0f; 
            System.Numerics.Vector3 localInertia = sphereshape.CalculateLocalInertia(mass); 

            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(mass, new OpenTKMotionState(_entity.GetComponent<TransformComponent>()), sphereshape, localInertia)
            {
                Friction = 0.7f,
                Restitution = 0.8f
            };
            RigidBody rigidBody = new RigidBody(rbInfo);

            _entity.AddComponent(new RigidBodyComponent(rigidBody));


        }
    }
}
