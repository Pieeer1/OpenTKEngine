using OpenTK.Mathematics;
using OpenTKEngine.Models;

namespace OpenTKEngine.Entities.Components
{
    public class TransformComponent : Component
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public TransformComponent(Vector3? postion = null, Quaternion? rotation = null, Vector3? scale = null) 
        {
            Position = postion ?? Vector3.Zero; 
            Rotation = rotation ?? Quaternion.Identity; 
            Scale = scale ?? Vector3.One;
        }
        public override void Init()
        {
            base.Init();
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Draw()
        {
            base.Draw();
        }
        public void RotateTo(Quaternion rotation)
        {
            Rotation = rotation;
        }
    }
}
