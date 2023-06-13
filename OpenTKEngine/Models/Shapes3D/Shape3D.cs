namespace OpenTKEngine.Models.Shapes3D
{
    public abstract class Shape3D
    {
        public abstract float[] _vertices { get; }

        public abstract void BindAndBuffer(Shader shader, out int vaoModel);



    }
}
