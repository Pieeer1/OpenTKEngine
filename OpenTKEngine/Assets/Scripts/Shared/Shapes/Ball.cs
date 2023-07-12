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

            CollisionShape sphereshape = new SphereShape(5.0f); // Specify the plane shape

            float mass = 1.0f; // Specify the mass of the object
            System.Numerics.Vector3 localInertia = sphereshape.CalculateLocalInertia(mass); // Calculate the local inertia based on the shape and mass

            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(mass, new OpenTKMotionState(_entity.GetComponent<TransformComponent>()), sphereshape, localInertia);
            RigidBody rigidBody = new RigidBody(rbInfo);
            rigidBody.CollisionFlags |= CollisionFlags.None; // Set the collision flag for a static object

            // Add the plane's rigid body to the dynamics world
            PhysicsService.Instance.DiscreteDynamicsWorld.AddRigidBody(rigidBody);
        }
    }
}
