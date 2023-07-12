using BulletSharp;
using OpenTKEngine.Models.Physics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class RigidBodyComponent : Component
    {
        private TransformComponent _transformComponent = null!;
        public RigidBodyComponent() 
        {
        
        }

        public override void Init()
        {
            base.Init();
            _transformComponent = Entity.AddComponent(new TransformComponent());

            // Create a rigid body for a dynamic object
            CollisionShape shape = new BoxShape(1.0f, 1.0f, 1.0f); // Specify the shape of the object (e.g., a box)
            float mass = 1.0f; // Specify the mass of the object
            System.Numerics.Vector3 localInertia = shape.CalculateLocalInertia(mass); // Calculate the local inertia based on the shape and mass

            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(mass, new OpenTKMotionState(_transformComponent), shape, localInertia);
            RigidBody rigidBody = new RigidBody(rbInfo);
            rigidBody.CollisionFlags |= CollisionFlags.CharacterObject;
            // Add the rigid body to the dynamics world
            PhysicsService.Instance.DiscreteDynamicsWorld.AddRigidBody(rigidBody);

            ManifoldPoint.ContactAdded += ManifoldPoint_ContactAdded;
        }

        private void ManifoldPoint_ContactAdded(ManifoldPoint cp, CollisionObjectWrapper colObj0Wrap, int partId0, int index0, CollisionObjectWrapper colObj1Wrap, int partId1, int index1)
        {
            double impulse = cp.AppliedImpulse;
            if (impulse < 0.4f) return;

            impulse -= 0.4f;

        }

        public override void Update() 
        {
            base.Init();
        }

    }
}
