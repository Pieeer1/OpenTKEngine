using BepuPhysics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class RigidBodyComponent : Component
    {
        protected TransformComponent _transformComponent = null!;
        protected float _mass;
        public BodyHandle _handle;
        private bool _isKinematic;
        public RigidBodyComponent(float mass, bool isKinematic)
        {
            _mass = mass;
            _isKinematic = isKinematic;
        }

        public override void Init()
        {
            base.Init();
            _transformComponent = Entity.AddComponent(new TransformComponent());

        }


        public override void Update()
        {
            base.Update();
            if (_isKinematic)
            {
                _transformComponent.Position = DataManipulationService.SystemVectorToOpenTKVector(PhysicsService.Instance.Simulation.Bodies[_handle].Pose.Position);
                _transformComponent.Rotation = DataManipulationService.SystemQuaternionToOpenTKQuaternion(PhysicsService.Instance.Simulation.Bodies[_handle].Pose.Orientation);

            }
        }
    }
}
