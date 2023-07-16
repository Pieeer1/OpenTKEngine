using BepuPhysics;
using BepuUtilities;
using static OpenTKEngine.Models.Physics.NarrowPhaseCallbacks;

namespace OpenTKEngine.Services
{
    public class PhysicsService : SingletonService<PhysicsService>
    {
        private PhysicsService() { }

        public Simulation Simulation { get; set; } = null!;
        public ThreadDispatcher ThreadDispatcher { get; set; } = null!;
        public CollidableProperty<SimpleMaterial> CollidableMaterials { get; set; } = null!; 
    }
}
