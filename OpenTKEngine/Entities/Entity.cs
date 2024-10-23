using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Enums;
using System.Reflection;
using System.Text.Json.Serialization;

namespace OpenTKEngine.Entities
{
    public class Entity
    {
        private List<Component> _components { get; set; } = [];
        private List<Entity> _childEntities { get; set; } = [];
        [JsonIgnore]
        public Transform Transform { get; set; } = new Transform();
        public SyncedTransforms SyncedTransforms { get; set; } = SyncedTransforms.None;
        public bool IsActive { get; private set; } = true;
        public bool IsVisible { get; set; } = true;
        public Layer Layer { get; set; }
        private Vector3 _previousParentPosition;
        private Quaternion _previousRotation;
        private Vector3 _previousScale;
        public void Update()
        {
            _childEntities.ForEach(child =>
            {
                Transform updatedTransform = child.Transform;
                if ((child.SyncedTransforms & SyncedTransforms.Position) != 0)
                {
                    Vector3 offset = updatedTransform.Position - _previousParentPosition;
                    updatedTransform.Position = Transform.Position + offset;
                }
                if ((child.SyncedTransforms & SyncedTransforms.Rotation) != 0)
                {
                    Quaternion offset = updatedTransform.Rotation - _previousRotation;
                    updatedTransform.Rotation = Transform.Rotation + offset;
                }
                if ((child.SyncedTransforms & SyncedTransforms.Scale) != 0)
                {
                    Vector3 offset = updatedTransform.Scale - _previousScale;
                    updatedTransform.Scale = Transform.Scale + offset;
                }
                child.Transform = updatedTransform;
            });

            _previousParentPosition = Transform.Position;
            _previousRotation = Transform.Rotation;
            _previousScale = Transform.Scale;

            foreach (var component in _components.ToList())
            {
                component.Update();
            }

        }
        public void Draw()
        {
            foreach (var component in _components.ToList())
            {
                component.Draw();
            }
        }        
        public void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            foreach (var component in _components.ToList())
            {
                component.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
            }
        }
        public Entity AddChildEntity(Entity e)
        {
            _childEntities.Add(e);
            return e;
        }
        public T AddComponent<T>(T newComponent) where T : Component
        {
            if (HasComponent<T>())
            { 
                return GetComponent<T>();
            }
            newComponent.Entity = this;

            newComponent.Init();
            _components.Add(newComponent);
            return newComponent;
        }
        public bool RemoveComponent<T>() where T : Component
        {
            if (HasComponent<T>())
            {
                _components.Add(GetComponent<T>());
                return true;
            }
            return false;
        }
        public void SetPropertyReferenceWithAttribute(Attribute att, object value)
        { 
            foreach (var component in _components)
            {
                IEnumerable<PropertyInfo> properties = component.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.GetCustomAttributes(true).Contains(att))
                    {
                        property.SetValue(component, value);
                    }
                }
            }
        }
        public T GetComponent<T>() where T : Component => (_components.First(x => x.GetType() == typeof(T)) as T ?? throw new InvalidCastException($"Could not Find any Components of Type {typeof(T).Name}"));
        public bool HasComponent<T>() where T : Component => _components.Any(x => x.GetType() == typeof(T));
        public bool HasChildrenEntities() => _childEntities.Any();
        public IEnumerable<Component> GetComponents() => _components.ToArray(); // copies.
        public IEnumerable<Entity> GetChildEntities() => _childEntities.ToArray(); // copies. 
        public void Destroy() => IsActive = false;
    }
}
