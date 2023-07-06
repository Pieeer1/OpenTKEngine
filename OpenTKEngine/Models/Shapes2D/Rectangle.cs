using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;
using System;
using System.Drawing;

namespace OpenTKEngine.Models.Shapes2D
{
    public class Rectangle : Shape2D
    {
        private readonly Vector2 _center;
        private readonly float _width;
        private readonly float _height;
        private readonly Vector3 _color;
        private int _texture;
        public Rectangle(Vector2 center, float width, float height, Vector3 color, Texture texture)
        {
            _center = center;
            _width = width;
            _height = height;
            _color = color;
            _texture = texture.Handle;
        }

        public override float[] _vertices { get => new float[]
            {
                _center.X - (_width/2), _center.Y - (_height/2), 0.0f, 0.0f,
                _center.X - (_width/2), _center.Y + (_height/2), 0.0f, 1.0f,
                _center.X + (_width/2), _center.Y - (_height/2), 1.0f, 1.0f,

                _center.X - (_width/2), _center.Y + (_height/2), 0.0f, 0.0f,
                _center.X + (_width/2), _center.Y + (_height/2), 1.0f, 1.0f,
                _center.X + (_width/2), _center.Y - (_height/2), 1.0f, 0.0f,
            };
        }
        public override Vector2 ScreenSize { get; set; }

        public override void BindAndBuffer(Shader shader)
        {
            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * 4, 0, BufferUsageHint.DynamicDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public override void Draw(Shader shader, TransformComponent transform)
        {
            shader.Use();

            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0.0f, ScreenSize.X, 0.0f, ScreenSize.Y, 1.0f, -1.0f);

            GL.UniformMatrix4(GL.GetUniformLocation(shader.Handle, "projection"), false, ref projection);
            GL.Uniform3(GL.GetUniformLocation(shader.Handle, "textColor"), _color.X, _color.Y, _color.Z);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindVertexArray(VAO);

            GL.BindTexture(TextureTarget.Texture2D, _texture);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferSubData(BufferTarget.ArrayBuffer, 0, sizeof(float) * 6 * 4, _vertices);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            DrawShape(shader, transform, () => GL.DrawArrays(PrimitiveType.Triangles, 0, 6));

            GL.BindVertexArray(0);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }
    }
}
