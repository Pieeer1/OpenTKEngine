using BulletSharp;
using OpenTK;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;

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
            worldTransform = OpenTKToBulletMatrix(openTKMatrix);
        }

        public override void SetWorldTransform(ref System.Numerics.Matrix4x4 worldTransform)
        {
            Matrix4 openTKMatrix = BulletToOpenTKMatrix(ref worldTransform);

            Vector3 translation = openTKMatrix.ExtractTranslation();
            Quaternion rotation = openTKMatrix.ExtractRotation();

            transform.Position = translation;
            transform.Rotation = rotation;
        }

        private Matrix4 BulletToOpenTKMatrix(ref System.Numerics.Matrix4x4 bulletMatrix)
        {
            return new Matrix4(
                (float)bulletMatrix.M11, (float)bulletMatrix.M12, (float)bulletMatrix.M13, (float)bulletMatrix.M14,
                (float)bulletMatrix.M21, (float)bulletMatrix.M22, (float)bulletMatrix.M23, (float)bulletMatrix.M24,
                (float)bulletMatrix.M31, (float)bulletMatrix.M32, (float)bulletMatrix.M33, (float)bulletMatrix.M34,
                (float)bulletMatrix.M41, (float)bulletMatrix.M42, (float)bulletMatrix.M43, (float)bulletMatrix.M44);
        }

        private System.Numerics.Matrix4x4 OpenTKToBulletMatrix(Matrix4 openTKMatrix)
        {
            System.Numerics.Matrix4x4 bulletMatrix = new System.Numerics.Matrix4x4();
            bulletMatrix.M11 = openTKMatrix.M11;
            bulletMatrix.M12 = openTKMatrix.M12;
            bulletMatrix.M13 = openTKMatrix.M13;
            bulletMatrix.M14 = openTKMatrix.M14;
            bulletMatrix.M21 = openTKMatrix.M21;
            bulletMatrix.M22 = openTKMatrix.M22;
            bulletMatrix.M23 = openTKMatrix.M23;
            bulletMatrix.M24 = openTKMatrix.M24;
            bulletMatrix.M31 = openTKMatrix.M31;
            bulletMatrix.M32 = openTKMatrix.M32;
            bulletMatrix.M33 = openTKMatrix.M33;
            bulletMatrix.M34 = openTKMatrix.M34;
            bulletMatrix.M41 = openTKMatrix.M41;
            bulletMatrix.M42 = openTKMatrix.M42;
            bulletMatrix.M43 = openTKMatrix.M43;
            bulletMatrix.M44 = openTKMatrix.M44;

            return bulletMatrix;
        }
    }
}
