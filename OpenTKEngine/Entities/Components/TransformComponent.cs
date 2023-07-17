using OpenTK.Mathematics;
using OpenTKEngine.Enums;
using OpenTKEngine.Models;
using System.Diagnostics;

namespace OpenTKEngine.Entities.Components
{
    public class TransformComponent : Component
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Vector3 Scale { get; set; }
        public SyncedTransforms SyncedTransforms { get; set; } = SyncedTransforms.None;
        private Vector3 _previousParentPosition;
        private Quaternion _previousRotation;
        private Vector3 _previousScale;
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
            
            Entity.ChildEntities.ForEach(e =>
            {
                var childTransform = e.GetComponent<TransformComponent>();
                if ((childTransform.SyncedTransforms & SyncedTransforms.Position) != 0)
                {
                    Vector3 offset = childTransform.Position - _previousParentPosition;
                    childTransform.Position = Position + offset;
                }
                if ((childTransform.SyncedTransforms & SyncedTransforms.Rotation) != 0)
                {
                    Quaternion offset = childTransform.Rotation - _previousRotation;
                    childTransform.Rotation = Rotation + offset;
                }
                if ((childTransform.SyncedTransforms & SyncedTransforms.Scale) != 0)
                {
                    Vector3 offset = childTransform.Scale - _previousScale;
                    childTransform.Scale = Scale + offset;
                }
            });
            _previousParentPosition = Position;
            _previousRotation = Rotation;
            _previousScale = Scale;
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
