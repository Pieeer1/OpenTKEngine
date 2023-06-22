using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKEngine.Entities
{
    public class Entity
    {
        private HashSet<Component> _components { get; set; } = new HashSet<Component>();
        public bool IsActive { get; private set; } = true;
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
                _components.Remove(GetComponent<T>());
                return true;
            }
            return false;
        }  
        public T GetComponent<T>() where T : Component => (_components.First(x => x.GetType() == typeof(T)) as T ?? throw new InvalidCastException($"Could not Find any Components of Type {typeof(T).Name}"));
        public bool HasComponent<T>() where T : Component => _components.Any(x => x.GetType() == typeof(T));
        public void Destroy() => IsActive = false;
    }
}
