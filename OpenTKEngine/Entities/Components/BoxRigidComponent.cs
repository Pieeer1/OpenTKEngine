using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class BoxRigidComponent : RigidBodyComponent
    {
        public Box Box { get; private set; }
        public BoxRigidComponent(Box box, float mass) : base(mass, true)
        {
            Box = box;
        }
        public override void Init()
        {
            base.Init();
            BodyInertia inertia = Box.ComputeInertia(_mass);
            _handle = PhysicsService.Instance.Simulation.Bodies.Add(BodyDescription.CreateDynamic(DataManipulationService.OpenTKVectorToSystemVector(_transformComponent.Position), inertia, PhysicsService.Instance.Simulation.Shapes.Add(Box), 0.01f));
            PhysicsService.Instance.CollidableMaterials.Allocate(_handle).FrictionCoefficient = 0.5f;
            PhysicsService.Instance.CollidableMaterials.Allocate(_handle).MaximumRecoveryVelocity = 2.0f;
            PhysicsService.Instance.CollidableMaterials.Allocate(_handle).SpringSettings = new BepuPhysics.Constraints.SpringSettings(30, 1 );

        }
    }
}
