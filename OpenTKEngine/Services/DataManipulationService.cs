using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;

namespace OpenTKEngine.Services
{
    public static class DataManipulationService
    {
        public static IEnumerable<float[]> GetTextureShadedArrayFromVectors(IEnumerable<Vector3> positions, IEnumerable<Vector3> normals, IEnumerable<Vector2> textures)
        {
            if (positions.Count() != normals.Count() || normals.Count() != textures.Count() || positions.Count() != textures.Count())
            {
                throw new InvalidOperationException("Cannot create float array of different sized Enumerables");
            }
            for (int i = 0; i < positions.Count(); i++)
            {
                yield return new float[] {
                positions.ElementAt(i).X,
                positions.ElementAt(i).Y,
                positions.ElementAt(i).Z,

                normals.ElementAt(i).X,
                normals.ElementAt(i).Y,
                normals.ElementAt(i).Z,

                textures.ElementAt(i).X,
                textures.ElementAt(i).Y };
            }
        }
        public static IEnumerable<T> Get1DFrom2D<T>(this IEnumerable<IEnumerable<T>> values)
        { 
            foreach (IEnumerable<T> enu in values)
            {
                foreach (T t in enu)
                {
                    yield return t;
                }
            }
        }
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
        public static string ParseResolution(Vector2i resolution) => $"{resolution.X}x{resolution.Y}";
        public static Vector2i ParseResolution(string resolution) => new Vector2i(int.Parse(resolution.Split('x')[0]), int.Parse(resolution.Split('x')[1]));
        public static void GetWorldTransform(out System.Numerics.Matrix4x4 worldTransform, TransformComponent transform)
        {
            Vector3 translation = transform.Position;
            Quaternion rotation = transform.Rotation;

            Matrix4 openTKMatrix = Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(translation);
            worldTransform = SystemMatrixToBulletMatrix(openTKMatrix);
        }

        public static void SetWorldTransform(ref System.Numerics.Matrix4x4 worldTransform, TransformComponent transform)
        {
            Matrix4 openTKMatrix = SystemMatrixToOpenTKMatrix(ref worldTransform);

            Vector3 translation = openTKMatrix.ExtractTranslation();
            Quaternion rotation = openTKMatrix.ExtractRotation();

            transform.Position = translation;
            transform.Rotation = rotation;
        }
        public static Matrix4 SystemMatrixToOpenTKMatrix(ref System.Numerics.Matrix4x4 bulletMatrix)
        {
            return new Matrix4(
                (float)bulletMatrix.M11, (float)bulletMatrix.M12, (float)bulletMatrix.M13, (float)bulletMatrix.M14,
                (float)bulletMatrix.M21, (float)bulletMatrix.M22, (float)bulletMatrix.M23, (float)bulletMatrix.M24,
                (float)bulletMatrix.M31, (float)bulletMatrix.M32, (float)bulletMatrix.M33, (float)bulletMatrix.M34,
                (float)bulletMatrix.M41, (float)bulletMatrix.M42, (float)bulletMatrix.M43, (float)bulletMatrix.M44);
        }

        public static System.Numerics.Matrix4x4 SystemMatrixToBulletMatrix(Matrix4 openTKMatrix)
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
        public static Vector3 SystemVectorToOpenTKVector(System.Numerics.Vector3 vec)
        {
            return new Vector3(vec.X, vec.Y, vec.Z);
        }
        public static System.Numerics.Vector3 OpenTKVectorToSystemVector(Vector3 vec)
        {
            return new System.Numerics.Vector3(vec.X, vec.Y, vec.Z);
        }        
        public static Vector4 SystemVectorToOpenTKVector(System.Numerics.Vector4 vec)
        {
            return new Vector4(vec.X, vec.Y, vec.Z, vec.W);
        }
        public static System.Numerics.Vector4 OpenTKVectorToSystemVector(Vector4 vec)
        {
            return new System.Numerics.Vector4(vec.X, vec.Y, vec.Z, vec.W);
        }
        public static System.Numerics.Quaternion OpenTKQuaternionToSystemQuaternion(Quaternion quat)
        {
            return new System.Numerics.Quaternion(quat.X, quat.Y, quat.Z, quat.W);
        }        
        public static Quaternion SystemQuaternionToOpenTKQuaternion(System.Numerics.Quaternion quat)
        {
            return new Quaternion(quat.X, quat.Y, quat.Z, quat.W);
        }
    }
}
