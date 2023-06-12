namespace OpenTKEngine.Entities
{
    public class EntityComponentManager
    {
        public EntityComponentManager() { }
        private HashSet<Entity> entities { get; set; } = new HashSet<Entity>();
        public void Update()
        {
            foreach (var entity in entities)
            {
                entity.Update();
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
    }
}
