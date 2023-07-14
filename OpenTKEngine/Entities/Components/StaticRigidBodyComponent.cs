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

            PhysicsService.Instance.Simulation.Statics.Add(new StaticDescription(new System.Numerics.Vector3(0, 0, 0), PhysicsService.Instance.Simulation.Shapes.Add(new Box(_transformComponent.Scale.X, 1.0f, _transformComponent.Scale.Y))));
        }
    }
}
