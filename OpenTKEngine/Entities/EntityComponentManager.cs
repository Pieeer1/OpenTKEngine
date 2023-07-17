using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Enums;

namespace OpenTKEngine.Entities
{
    public class EntityComponentManager 
    {
        public EntityComponentManager() { }
        private List<Entity> entities { get; set; } = new List<Entity>();
        public void Update()
        {
            foreach (var entity in entities)
            {
                entity.Update();
                foreach (Entity childEntity in entity.ChildEntities)
                {
                    childEntity.Update();
                }
            }
        }        
        public void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            foreach (var entity in entities)
            {
                entity.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
                foreach (Entity childEntity in entity.ChildEntities)
                {
                    childEntity.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
                }
            }
        }
        public void Draw()
        {
            foreach (var entity in entities.Where(x => x.IsVisible))
            {
                entity.Draw();
                foreach (Entity childEntity in entity.ChildEntities)
                {
                    childEntity.Draw();
                }
            }
        }        
        public void Refresh()
        {
            foreach (var entity in entities)
            {
                if (!entity.IsActive)
                {
                    entity.Destroy();
                    foreach (Entity childEntity in entity.ChildEntities)
                    {
                        childEntity.Destroy();
                    }
                }
            }
        }
        public void UpdateEntityLayer(Entity entity, Layer layer)
        {
            entity.Layer = layer;
        }
        public Entity AddEntity(Layer layer = Layer.None)
        {
            Entity e = new Entity();
            e.Layer = layer;
            entities.Add(e);
            return e;
        }
        public IEnumerable<Entity> GetEntities() => entities;

        public IEnumerable<Entity> GetEntitiesWithType<T>() where T : Component
        {
            return entities.Where(x => x.HasComponent<T>());
        }
        public void SetComponentReferencesWithAttribute(Attribute att, object value)
        {
            foreach (var entity in entities)
            {
                entity.SetPropertyReferenceWithAttribute(att, value);
            }
        }
    }
}
