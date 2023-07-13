using BulletSharp;
using OpenTKEngine.Models.Physics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class RigidBodyComponent : Component
    {
        private TransformComponent _transformComponent = null!;
        private readonly CollisionShape _collisionShape = null!;
        private float _mass;
        private CollisionFlags _collisionFlags;
        public RigidBody RigidBody { get; private set; } = null!;
        public RigidBodyComponent(CollisionShape collisionShape, float mass, CollisionFlags? collisionFlags = null)
        {
            _collisionShape = collisionShape;
            _mass = mass;
            _collisionFlags = collisionFlags ?? CollisionFlags.None;
        }
        public RigidBodyComponent(RigidBody rigidBody, CollisionFlags? collisionFlags = null)
        {
            RigidBody = rigidBody;
            _collisionFlags = collisionFlags ?? CollisionFlags.None;
        }
        public override void Init()
        {
            base.Init();
            _transformComponent = Entity.AddComponent(new TransformComponent());
            if (RigidBody is null)
            {
                System.Numerics.Vector3 localInertia = _collisionShape.CalculateLocalInertia(_mass);

                RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(_mass, new OpenTKMotionState(_transformComponent), _collisionShape, localInertia);
                RigidBody = new RigidBody(rbInfo);
            }
            RigidBody.CollisionFlags = _collisionFlags;
            PhysicsService.Instance.DiscreteDynamicsWorld.AddRigidBody(RigidBody);
        }


        public override void Update() 
        {
            base.Init();
        }

    }
}
