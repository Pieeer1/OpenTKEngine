using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class BoxRigidComponent : RigidBodyComponent
    {
        private Box _box;
        public BoxRigidComponent(Box box, float mass) : base(mass, true)
        {
            _box = box;
        }
        public override void Init()
        {
            base.Init();
            BodyInertia inertia = _box.ComputeInertia(_mass);
            _handle = PhysicsService.Instance.Simulation.Bodies.Add(BodyDescription.CreateDynamic(DataManipulationService.OpenTKVectorToSystemVector(_transformComponent.Position), inertia, PhysicsService.Instance.Simulation.Shapes.Add(_box), 0.01f));

        }
    }
}
