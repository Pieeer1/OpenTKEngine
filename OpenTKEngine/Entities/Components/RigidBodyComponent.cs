﻿using BulletSharp;
using OpenTKEngine.Models.Physics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class RigidBodyComponent : Component
    {
        private TransformComponent _transformComponent = null!;
        private readonly CollisionShape _collisionShape;
        private float _mass;
        private CollisionFlags _collisionFlags;

        public RigidBodyComponent(CollisionShape collisionShape, float mass, CollisionFlags collisionFlags)
        {
            _collisionShape = collisionShape;
            _mass = mass;
            _collisionFlags = collisionFlags;
        }

        public override void Init()
        {
            base.Init();
            _transformComponent = Entity.AddComponent(new TransformComponent());

            System.Numerics.Vector3 localInertia = _collisionShape.CalculateLocalInertia(_mass);

            RigidBodyConstructionInfo rbInfo = new RigidBodyConstructionInfo(_mass, new OpenTKMotionState(_transformComponent), _collisionShape, localInertia);
            RigidBody rigidBody = new RigidBody(rbInfo);
            rigidBody.CollisionFlags = _collisionFlags;

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
