using BulletSharp;

namespace OpenTKEngine.Services
{
    public class PhysicsService : SingletonService<PhysicsService>
    {
        private PhysicsService() { }

        public DiscreteDynamicsWorld DiscreteDynamicsWorld { get; set; } = null!;
    }
}
