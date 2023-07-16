using BepuPhysics;
using BepuPhysics.Collidables;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class StaticRigidBodyComponent : RigidBodyComponent
    {
        public StaticRigidBodyComponent() : base(0.0f, false)
        {

        }
        public override void Init()
        {
            base.Init();

            PhysicsService.Instance.Simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(0, 0, 0), PhysicsService.Instance.Simulation.Shapes.Add(new Box(_transformComponent.Scale.X * 2500, 1.0f, _transformComponent.Scale.Y * 2500))));
            PhysicsService.Instance.CollidableMaterials.Allocate(_handle).FrictionCoefficient = 0.5f;
            PhysicsService.Instance.CollidableMaterials.Allocate(_handle).MaximumRecoveryVelocity = 2.0f;
            PhysicsService.Instance.CollidableMaterials.Allocate(_handle).SpringSettings = new BepuPhysics.Constraints.SpringSettings(30, 1);
        }
    }
}
