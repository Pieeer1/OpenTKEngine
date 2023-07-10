using OpenTK.Graphics.OpenGL4;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.Shapes3D;

namespace OpenTKEngine.Models.Shapes3D.Models
{
    public class Mesh : Shape3D
    {
        private readonly float[] _vertRef;
        private readonly uint[] _indicesRef;
        public Mesh(float[] vertRef, uint[] indicesRef)
        {
            _vertRef = vertRef;
            _indicesRef = indicesRef;
        }

        public override float[] _vertices { get => _vertRef; }
        public uint[] indices { get => _indicesRef; }

        public override void BindAndBuffer(Shader shader)
        {
            ElementArrayBuffer(shader, indices);
        }

        public override void Draw(Shader shader, TransformComponent transform)
        {
            DrawShape(shader, transform, () => GL.DrawElements(PrimitiveType.Triangles, indices.Count(), DrawElementsType.UnsignedInt, 0));
        }
    }
}
