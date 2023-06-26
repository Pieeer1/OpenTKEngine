using OpenTKEngine.Entities.Components;

namespace OpenTKEngine.Models
{
    public abstract class RenderableObject
    {
        public int VAO { get; set; }
        public int VBO { get; set; }
        public int? EBO { get; set; }
        public abstract void BindAndBuffer(Shader shader);
        public abstract void Draw(Shader shader, TransformComponent transform);
    }
}
