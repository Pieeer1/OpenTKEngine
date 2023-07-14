using BepuPhysics;
using BepuUtilities;

namespace OpenTKEngine.Services
{
    public class PhysicsService : SingletonService<PhysicsService>
    {
        private PhysicsService() { }

        public Simulation Simulation { get; set; } = null!;
        public ThreadDispatcher ThreadDispatcher { get; set; } = null!;
    }
}
