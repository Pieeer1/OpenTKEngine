using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKEngine.Entities
{
    public class EntityComponentManager
    {
        public Vector2 ScreenSize { get; set; }
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
        private HashSet<Entity> entities { get; set; } = new HashSet<Entity>();
        public void Update()
        {
            foreach (var entity in entities)
            {
                entity.Update();
            }
        }        
        public void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            foreach (var entity in entities)
            {
                entity.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
            }
        }
        public void Draw()
        {
            foreach (var entity in entities)
            {
                entity.Draw();
            }
        }        
        public void Refresh()
        {
            foreach (var entity in entities)
            {
                if (!entity.IsActive)
                {
                    entity.Destroy();
                }
            }
        }

        public Entity AddEntity()
        {
            Entity e = new Entity();
            entities.Add(e);
            return e;
        }
        public HashSet<Entity> GetEntities() => entities;

        public IEnumerable<Entity> GetEntitiesWithType<T>() where T : Component
        {
            return entities.Where(x => x.HasComponent<T>());
        }
    }
}
