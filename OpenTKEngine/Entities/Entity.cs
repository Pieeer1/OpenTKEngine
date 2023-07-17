using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Enums;
using System.Reflection;

namespace OpenTKEngine.Entities
{
    public class Entity
    {
        private List<Component> _components { get; set; } = new List<Component>();
        public bool IsActive { get; private set; } = true;
        public bool IsVisible { get; set; } = true;
        public Layer Layer { get; set; }
        public void Update()
        {
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
        public void Destroy() => IsActive = false;
    }
}
