using OpenTKEngine.Entities;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;
using static OpenTKEngine.Models.Constants;
using OpenTKEngine.Models;
using BulletSharp;
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

            //Vector3 initialOrientation = new Vector3(0.0f, 0.0f, 1.0f); // default orientation
            //Vector3 targetOrientation = new Vector3(0.0f, 1.0f, 0.0f);

            //Quaternion rotationQuaternion = Quaternion.FromAxisAngle(Vector3.Cross(initialOrientation, targetOrientation), MathHelper.DegreesToRadians(90f));

            _entity.AddComponent(new ModelComponent(_shader, new Models.Shapes3D.Models.Model($"{AssetRoutes.Models}/plane.dae"), position, rotation, scale != null ? new Vector3(scale.Value.X, 0.01f, scale.Value.Y) : new Vector3(1.0f, 0.01f, 1.0f), new List<Texture>()
            {
                Texture.LoadFromFile($"{AssetRoutes.Textures}/planegray.png")
            }));

            CollisionShape planeShape = new BoxShape(new System.Numerics.Vector3(10.0f, 0.01f, 10.0f)); ; // Specify the plane shape
            float planeMass = 0.0f; // Set the mass to zero for a static object

            RigidBodyConstructionInfo planeRbInfo = new RigidBodyConstructionInfo(planeMass, null, planeShape);
            RigidBody planeRigidBody = new RigidBody(planeRbInfo);
            planeRigidBody.Restitution = 0.5f;
            planeRigidBody.Friction = 0.5f;
            planeRigidBody.CollisionFlags |= CollisionFlags.StaticObject; // Set the collision flag for a static object

            // Add the plane's rigid body to the dynamics world
            PhysicsService.Instance.DiscreteDynamicsWorld.AddRigidBody(planeRigidBody);
        }
    }
}
