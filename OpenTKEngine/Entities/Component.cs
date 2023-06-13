namespace OpenTKEngine.Entities
{
    public class Component
    {
        public Entity Entity { get; set; } = null!;

        public virtual void Init()
        { 
        
        }
        public virtual void Update() 
        {
        
        }
        public virtual void Draw()
        { 
            
        }
        public virtual void PostDraw()
        { 
        
        }
    }
}
