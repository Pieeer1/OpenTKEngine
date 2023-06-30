using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;

namespace OpenTKEngine.Models.Shapes2D
{
    public abstract class Shape2D : RenderableObject
    {
        public abstract float[] _vertices { get; }
        public abstract Vector2 ScreenSize { get; set; }
        private protected void ArrayBuffer(Shader shader) //TODO ADD A VALUE FOR THE TEXTURED ONE AND THE COLORED ONE FOR BOTH SHAPE2D AND 3D
        {
            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            var positionLocation = shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var colorLocation = shader.GetAttribLocation("aColor");
            GL.EnableVertexAttribArray(colorLocation);
            GL.VertexAttribPointer(colorLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 2 * sizeof(float));

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }
        private protected void DrawShape(Shader shader, TransformComponent transform, Action glDraw) // ONLY HANDLES UI ELEMENTS: TODO ADD NON UI 2D ELEMENTS  -> -> MAYBE JUST CHANGE THIS TO UI2DSHAPE?
        {
            GL.BindVertexArray(VAO);

            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0.0f, ScreenSize.X, 0.0f, ScreenSize.Y, 1.0f, -1.0f);

            shader.SetMatrix4("projection", projection);

            glDraw.Invoke();

        }
    }
}
