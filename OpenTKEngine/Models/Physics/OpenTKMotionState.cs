using BulletSharp;
using OpenTK;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.Physics
{


    public class OpenTKMotionState : MotionState
    {
        private TransformComponent transform;

        public OpenTKMotionState(TransformComponent initialTransform)
        {
            transform = initialTransform;
        }

        public override void GetWorldTransform(out System.Numerics.Matrix4x4 worldTransform)
        {
            Vector3 translation = transform.Position;
            Quaternion rotation = transform.Rotation;

            Matrix4 openTKMatrix = Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(translation);
            worldTransform = DataManipulationService.OpenTKToBulletMatrix(openTKMatrix);
        }

        public override void SetWorldTransform(ref System.Numerics.Matrix4x4 worldTransform)
        {
            Matrix4 openTKMatrix = DataManipulationService.BulletToOpenTKMatrix(ref worldTransform);

            Vector3 translation = openTKMatrix.ExtractTranslation();
            Quaternion rotation = openTKMatrix.ExtractRotation();

            transform.Position = translation;
            transform.Rotation = rotation;
        }
    }
}
