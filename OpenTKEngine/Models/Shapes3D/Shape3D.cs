using OpenTKEngine.Entities.Components;

namespace OpenTKEngine.Models.Shapes3D
{
    public abstract class Shape3D : RenderableObject
    {
        public abstract float[] _vertices { get; }

    }
}
