using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTKEngine.Entities.Components;

namespace OpenTKEngine.Models.Shapes2D
{
    public class Rectangle : Shape2D
    {
        private readonly Vector2 _center;
        private readonly float _width;
        private readonly float _height;
        private readonly Vector3 _color;

        public Rectangle(Vector2 center, float width, float height, Vector3 color)
        {
            _center = center;
            _width = width;
            _height = height;
            _color = color;
        }

        public override float[] _vertices { get => new float[]
            {
                _center.X - (_width/2), _center.Y - (_height/2), _color.X, _color.Y, _color.Z,
                _center.X - (_width/2), _center.Y + (_height/2), _color.X, _color.Y, _color.Z,
                _center.X + (_width/2), _center.Y - (_height/2), _color.X, _color.Y, _color.Z,

                _center.X - (_width/2), _center.Y + (_height/2), _color.X, _color.Y, _color.Z,
                _center.X + (_width/2), _center.Y + (_height/2), _color.X, _color.Y, _color.Z,
                _center.X + (_width/2), _center.Y - (_height/2), _color.X, _color.Y, _color.Z,
            };
        }
        public override Vector2 ScreenSize { get; set; }

        public override void BindAndBuffer(Shader shader)
        {
            ArrayBuffer(shader);
        }

        public override void Draw(Shader shader, TransformComponent transform)
        {
            DrawShape(shader, transform, () => GL.DrawArrays(PrimitiveType.Triangles, 0, 6));
        }
    }
}
