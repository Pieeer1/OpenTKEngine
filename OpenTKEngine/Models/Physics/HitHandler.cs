using BepuPhysics;
using BepuPhysics.Collidables;
using System.Numerics;
using BepuUtilities.Memory;
using System.Runtime.CompilerServices;
using BepuPhysics.Trees;
using BepuUtilities.Collections;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.Physics
{
    public struct RayHit
    {
        public Vector3 Normal;
        public float T;
        public CollidableReference Collidable;
        public bool Hit;
    }
    public unsafe struct HitHandler : IRayHitHandler
    {
        public Buffer<RayHit> Hits;
        public int* IntersectionCount;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowTest(CollidableReference collidable)
        {
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool AllowTest(CollidableReference collidable, int childIndex)
        {
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnRayHit(in RayData ray, ref float maximumT, float t, in Vector3 normal, CollidableReference collidable, int childIndex)
        {
            maximumT = t;
            ref var hit = ref Hits[ray.Id];
            if (t < hit.T)
            {
                if (hit.T == float.MaxValue)
                    ++*IntersectionCount;
                hit.Normal = normal;
                hit.T = t;
                hit.Collidable = collidable;
                hit.Hit = true;
            }
        }
    }
    public struct BasicRayHit
    {
        public Vector3 Location;
        public Vector3 Normal;
    }
    public struct BasicHitHandler : IRayHitHandler
    {
        public bool HasHit;
        public BasicRayHit HitData;

        public bool AllowTest(CollidableReference collidable)
        {
            return true; // Optionally, you can filter bodies to test against here
        }

        public bool AllowTest(CollidableReference collidable, int childIndex)
        {
            return true; // Optionally, you can filter bodies to test against here
        }

        public void OnRayHit(in RayData ray, ref float maximumT, float t, in Vector3 normal, CollidableReference collidable, int childIndex)
        {
            HasHit = true;
            HitData.Location = PhysicsService.Instance.Simulation.Bodies[collidable.BodyHandle].Pose.Position;
            HitData.Normal = normal;
        }
    }
}
