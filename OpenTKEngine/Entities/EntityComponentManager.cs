using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKEngine.Entities
{
    public class EntityComponentManager
    {
        private static EntityComponentManager? _instance;
        public static EntityComponentManager Instance
        {
            get
            {
                if (_instance is null)
                {
                    _instance = new EntityComponentManager();
                }
                return _instance;
            }
        }
        private EntityComponentManager() { }
        private Dictionary<Entity, uint> entities { get; set; } = new Dictionary<Entity, uint>();
        public void Update()
        {
            foreach (var entity in entities)
            {
                entity.Key.Update();
            }
        }        
        public void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            foreach (var entity in entities)
            {
                entity.Key.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
            }
        }
        public void Draw()
        {
            foreach (var entity in entities.Where(x => x.Key.IsVisible).OrderByDescending(x => x.Value))
            {
                entity.Key.Draw();
            }
        }        
        public void Refresh()
        {
            foreach (var entity in entities.OrderByDescending(x => x.Value))
            {
                if (!entity.Key.IsActive)
                {
                    entity.Key.Destroy();
                }
            }
        }
        public void UpdateEntityLayer(Entity entity, uint layer)
        {
            if (entities.ContainsKey(entity))
            {
                entities[entity] = layer;
            }
        }
        public Entity AddEntity(uint layer = 0)
        {
            Entity e = new Entity();
            entities.Add(e, layer);
            return e;
        }
        public IEnumerable<Entity> GetEntities() => entities.Keys;

        public IEnumerable<Entity> GetEntitiesWithType<T>() where T : Component
        {
            return entities.Keys.Where(x => x.HasComponent<T>());
        }
        public void SetComponentReferencesWithAttribute(Attribute att, object value)
        {
            foreach (var entity in entities)
            {
                entity.Key.SetPropertyReferenceWithAttribute(att, value);
            }
        }
    }
}
