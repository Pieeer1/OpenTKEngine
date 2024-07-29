using BepuPhysics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class RigidBodyComponent : Component
    {
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
        }


        public override void Update()
        {
            base.Update();
            if (_isKinematic)
            {
                Entity.Transform = new Transform(
                    DataManipulationService.SystemVectorToOpenTKVector(PhysicsService.Instance.Simulation.Bodies[_handle].Pose.Position),
                    DataManipulationService.SystemQuaternionToOpenTKQuaternion(PhysicsService.Instance.Simulation.Bodies[_handle].Pose.Orientation),
                    Entity.Transform.Scale
                    );
            }
        }
    }
}
