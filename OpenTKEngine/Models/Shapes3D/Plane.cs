using OpenTK.Graphics.OpenGL4;
using OpenTKEngine.Entities.Components;

namespace OpenTKEngine.Models.Shapes3D
{
    public class Plane : Shape3D
    {
        public override float[] _vertices { get => new float[]
        {
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,
             0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
             0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f,
            -0.5f,  0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f,
            -0.5f, -0.5f, -0.5f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f,            
        }; }

        public override void BindAndBuffer(Shader shader)
        {
            ArrayBuffer(shader);
        }

        public override void Draw(Shader shader, TransformComponent transform)
        {
            DrawShape(shader, transform, () => GL.DrawArrays(PrimitiveType.Triangles, 0, 36));
        }
    }
}
