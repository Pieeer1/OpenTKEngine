using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.Shapes3D
{
    public class Plane : Shape3D
    {
        private uint _dimensions;
        public Plane(uint dimensions) 
        {
            _dimensions = dimensions < 2 ? 2 : dimensions; // lower than 2 removes the entire plane
        }

        public override float[] _vertices { get => CreatePlaneVertices(); }
        public uint[] _indices { get => CreatePlaneIndices(); }
        public override void BindAndBuffer(Shader shader)
        {
            ElementArrayBuffer(shader, _indices);
        }

        public override void Draw(Shader shader, TransformComponent transform)
        {
            DrawShape(shader, transform, () => GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0));
        }

        private float[] CreatePlaneVertices()
        {
            List<Vector3> positions = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> textures = new List<Vector2>();

            uint halfDim = _dimensions / 2;
            for (int i = 0; i < _dimensions; i++)
            {
                for (int j = 0; j < _dimensions; j++)
                {
                    float xSeg = j - halfDim;
                    float ySeg = 0.0f;
                    float zSeg = i - halfDim;

                    positions.Add(new Vector3(xSeg, ySeg, zSeg));
                    normals.Add(new Vector3(xSeg, ySeg, zSeg));
                    textures.Add(new Vector2(i, j)); // currently repeats the image over the size of the texture
                }
            }

            return DataManipulationService.GetTextureShadedArrayFromVectors(positions, normals, textures).Get1DFrom2D().ToArray();
        }
        private uint[] CreatePlaneIndices()
        {
            List<uint> indices = new List<uint>();
            for (int i = 0; i < _dimensions; i++)
            {
                for (int j = 0; j < _dimensions; j++)
                {
                    indices.Add(_dimensions * (uint)i + (uint)j);
                    indices.Add(_dimensions * (uint)i + (uint)j + _dimensions);
                    indices.Add(_dimensions * (uint)i + (uint)j + _dimensions + 1);

                    indices.Add(_dimensions * (uint)i + (uint)j);
                    indices.Add(_dimensions * (uint)i + (uint)j + _dimensions + 1);
                    indices.Add(_dimensions * (uint)i + (uint)j + 1);

                }
            }
            return indices.ToArray();
        }
    }
}
