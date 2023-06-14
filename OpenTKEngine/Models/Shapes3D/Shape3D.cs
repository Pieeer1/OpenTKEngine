using OpenTKEngine.Entities.Components;

namespace OpenTKEngine.Models.Shapes3D
{
    public abstract class Shape3D
    {
        public abstract float[] _vertices { get; }
        public abstract int VAO { get; set; }
        public abstract void BindAndBuffer(Shader shader);
        public abstract void Draw(Shader shader, TransformComponent transform);


    }
}
