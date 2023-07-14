using BepuPhysics;
using BepuUtilities;
using System.Numerics;

namespace OpenTKEngine.Models.Physics
{
    public struct PoseIntegratorCallbacks : IPoseIntegratorCallbacks
    {
        public void Initialize(Simulation simulation)
        {

        }

        public readonly AngularIntegrationMode AngularIntegrationMode => AngularIntegrationMode.Nonconserving;

        public readonly bool AllowSubstepsForUnconstrainedBodies => false;

        public readonly bool IntegrateVelocityForKinematics => false;

        public Vector3 Gravity;

        public PoseIntegratorCallbacks(Vector3 gravity) : this()
        {
            Gravity = gravity;
        }

        
        Vector3Wide gravityWideDt;

        public void PrepareForIntegration(float dt)
        {
            gravityWideDt = Vector3Wide.Broadcast(Gravity * dt);
        }
        public void IntegrateVelocity(Vector<int> bodyIndices, Vector3Wide position, QuaternionWide orientation, BodyInertiaWide localInertia, Vector<int> integrationMask, int workerIndex, Vector<float> dt, ref BodyVelocityWide velocity)
        {
            velocity.Linear += gravityWideDt;
        }

    }
}
