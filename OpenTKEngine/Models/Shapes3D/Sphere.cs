using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.Shapes3D
{
    public class Sphere : Shape3D
    {
        public override float[] _vertices { get => CreateSphere(); }
        public uint[] _indices { get => CreateSphereIndices(); }

        private const int xSegments = 64;
        private const int ySegments = 64;

        public override void BindAndBuffer(Shader shader)
        {
            ElementArrayBuffer(shader, _indices);
        }

        private float[] CreateSphere()
        {
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();

            for (int x = 0; x <= xSegments; ++x)
            {
                for (int y = 0; y <= ySegments; ++y)
                {
                    float xSegment = (float)x / (float)xSegments;
                    float ySegment = (float)y / (float)ySegments;
                    float xPos = (float)(MathHelper.Cos(xSegment * 2.0f * MathHelper.Pi) * MathHelper.Sin(ySegment * MathHelper.Pi));
                    float yPos = (float)MathHelper.Cos(ySegment * MathHelper.Pi);
                    float zPos = (float)(MathHelper.Sin(xSegment * 2.0f * MathHelper.Pi) * MathHelper.Sin(ySegment * MathHelper.Pi));

                    positions.Add(new Vector3(xPos, yPos, zPos));
                    normals.Add(new Vector3(xPos, yPos, zPos));
                    textures.Add(new Vector2(xSegment, ySegment));
                }
            }

            return DataManipulationService.GetTextureShadedArrayFromVectors(positions, normals, textures).Get1DFrom2D().ToArray();
        }
        private uint[] CreateSphereIndices()
        {
            List<uint> indices = new List<uint>();
            for (int y = 0; y < ySegments; y++)
            {
                if (y % 2 == 0)
                {
                    for (uint x = 0; x <= xSegments; ++x)
                    {
                        indices.Add((uint)(y * (xSegments + 1) + x));
                        indices.Add((uint)((y + 1) * (xSegments + 1) + x));
                    }
                }
                else
                {
                    for (int x = xSegments; x >= 0; --x)
                    {
                        indices.Add((uint)((y + 1) * (xSegments + 1) + x));
                        indices.Add((uint)(y * (xSegments + 1) + x));
                    }
                }
            }

            return indices.ToArray();
        }

        public override void Draw(Shader shader, TransformComponent transform)
        {
            DrawShape(shader, transform, () => GL.DrawElements(PrimitiveType.TriangleStrip, _indices.Length, DrawElementsType.UnsignedInt, 0));
        }
    }
}
