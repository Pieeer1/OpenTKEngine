using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class SphereRigidBodyComponent : RigidBodyComponent
    {
        private Sphere _sphere;
        public SphereRigidBodyComponent(Sphere sphere, float mass) : base(mass)
        {
            _sphere = sphere;
        }
        public override void Init()
        {
            base.Init();
            BodyInertia inertia = _sphere.ComputeInertia(_mass);
            _handle = PhysicsService.Instance.Simulation.Bodies.Add(BodyDescription.CreateDynamic(DataManipulationService.OpenTKVectorToSystemVector(_transformComponent.Position), inertia, PhysicsService.Instance.Simulation.Shapes.Add(_sphere), 0.01f));
        }
    }
}
