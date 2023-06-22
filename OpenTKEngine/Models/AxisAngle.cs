using OpenTK.Mathematics;

namespace OpenTKEngine.Models
{
    public struct AxisAngle
    {
        public Vector3 Axis { get; set; }
        public float Angle { get; set; }
        public static AxisAngle Identity = new AxisAngle(Vector3.One, 0.0f);
        public AxisAngle(Vector3 axis, float angle)
        {
            Axis = axis;
            Angle = angle;
        }
        public AxisAngle(float x, float y, float z, float angle)
        { 
            Axis = new Vector3(x, y, z);
            Angle = angle;
        }
    }
}
