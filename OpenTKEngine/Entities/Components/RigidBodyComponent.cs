using OpenTKEngine.Services;

namespace OpenTKEngine.Entities.Components
{
    public class RigidBodyComponent : Component
    {
        private TransformComponent _transformComponent = null!;
        public RigidBodyComponent() 
        {
        
        }

        public override void Init()
        {
            base.Init();
            _transformComponent = Entity.AddComponent(new TransformComponent());
        }

        public override void Update() 
        {
            base.Init();
            //todo -> check grounded
            _transformComponent.Position = new OpenTK.Mathematics.Vector3(_transformComponent.Position.X, _transformComponent.Position.Y - ((float)TimeService.Instance.DeltaTime * 1.0f), _transformComponent.Position.Z);
        }

    }
}
