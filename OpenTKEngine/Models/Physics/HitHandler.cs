using BepuPhysics;
using BepuPhysics.Collidables;
using System.Numerics;
using BepuUtilities.Memory;
using System.Runtime.CompilerServices;
using BepuPhysics.Trees;
using BepuUtilities;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.Physics
{
    public struct Ray
    {
        public Vector3 Origin;
        public float MaximumT;
        public Vector3 Direction;
    }
    public struct RayHit
    {
        public Vector3 Normal;
        public float T;
        public CollidableReference Collidable;
        public bool Hit;
    }

    public unsafe struct StaticHitHandler : IRayHitHandler //only collides with static objects
    {
        public RayHit RayHit;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowTest(CollidableReference collidable)
        {
            if (collidable.Mobility == CollidableMobility.Static) { return true; }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowTest(CollidableReference collidable, int childIndex)
        {
            if (collidable.Mobility == CollidableMobility.Static) { return true; }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnRayHit(in RayData ray, ref float maximumT, float t, in Vector3 normal, CollidableReference collidable, int childIndex)
        {
            if (t < RayHit.T)
            {
                RayHit = new RayHit()
                {
                    Normal = normal,
                    T = t,
                    Collidable = collidable,
                    Hit = true
                };
            }
        }
    }
}
