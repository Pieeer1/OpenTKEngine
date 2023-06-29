using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Reflection;

namespace OpenTKEngine.Entities
{
    public class Entity
    {
        private Dictionary<Component, uint> _components { get; set; } = new Dictionary<Component, uint>();
        public bool IsActive { get; private set; } = true;
        public bool IsVisible { get; set; } = true;

        public void Update()
        {
            foreach (var component in _components.ToList().OrderByDescending(x => x.Value))
            {
                component.Key.Update();
            }
        }
        public void Draw()
        {
            foreach (var component in _components.ToList().OrderByDescending(x => x.Value))
            {
                component.Key.Draw();
            }
        }        
        public void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            foreach (var component in _components.ToList().OrderByDescending(x => x.Value))
            {
                component.Key.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
            }
        }
        public T AddComponent<T>(T newComponent, uint layer = 0) where T : Component
        {
            if (HasComponent<T>())
            { 
                return GetComponent<T>();
            }
            newComponent.Entity = this;
            newComponent.Init();
            _components.Add(newComponent, layer);
            return newComponent;
        }
        public bool RemoveComponent<T>() where T : Component
        {
            if (HasComponent<T>())
            {
                _components.Remove(GetComponent<T>());
                return true;
            }
            return false;
        }
        public void SetPropertyReferenceWithAttribute(Attribute att, object value)
        { 
            foreach (var component in _components.Keys)
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
        public T GetComponent<T>() where T : Component => (_components.Keys.First(x => x.GetType() == typeof(T)) as T ?? throw new InvalidCastException($"Could not Find any Components of Type {typeof(T).Name}"));
        public bool HasComponent<T>() where T : Component => _components.Keys.Any(x => x.GetType() == typeof(T));
        public void Destroy() => IsActive = false;
    }
}
