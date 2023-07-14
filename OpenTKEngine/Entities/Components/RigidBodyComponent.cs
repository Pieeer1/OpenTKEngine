using BepuPhysics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class RigidBodyComponent : Component
    {
        protected TransformComponent _transformComponent = null!;
        protected float _mass;
        protected BodyHandle _handle;

        public RigidBodyComponent(float mass)
        {
            _mass = mass;
        }

        public override void Init()
        {
            base.Init();
            _transformComponent = Entity.AddComponent(new TransformComponent());

        }


        public override void Update()
        {
            base.Update();
            if (_handle.Value != 0)
            {
                _transformComponent.Position = DataManipulationService.SystemVectorToOpenTKVector(PhysicsService.Instance.Simulation.Bodies[_handle].Pose.Position);
                _transformComponent.Rotation = DataManipulationService.SystemQuaternionToOpenTKQuaternion(PhysicsService.Instance.Simulation.Bodies[_handle].Pose.Orientation);
            }
        }
    }
}
