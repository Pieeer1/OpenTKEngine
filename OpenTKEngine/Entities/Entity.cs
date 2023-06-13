namespace OpenTKEngine.Entities
{
    public class Entity
    {
        private HashSet<Component> _components { get; set; } = new HashSet<Component>();
        public bool IsActive { get; private set; } = true;
        public void Update()
        {
            foreach (var component in _components)
            {
                component.Update();
            }
        }
        public void Draw()
        {
            foreach (var component in _components)
            {
                component.Draw();
            }
        }        
        public void PostDraw()
        {
            foreach (var component in _components)
            {
                component.PostDraw();
            }
        }
        public T AddComponent<T>(T newComponent) where T : Component
        {
            newComponent.Entity = this;
            newComponent.Init();
            _components.Add(newComponent);
            return newComponent;
        }
        public T GetComponent<T>() where T : Component => (_components.First(x => x.GetType() == typeof(T)) as T ?? throw new InvalidCastException($"Could not Find any Components of Type {typeof(T).Name}"));
        public bool HasComponent<T>() where T : Component => _components.Any(x => x.GetType() == typeof(T));
        public void Destroy() => IsActive = false;
    }
}
