using OpenTKEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTKEngine.Models
{
    public abstract class RenderableObject
    {
        public abstract int VAO { get; set; }
        public abstract void BindAndBuffer(Shader shader);
        public abstract void Draw(Shader shader, TransformComponent transform);
    }
}
