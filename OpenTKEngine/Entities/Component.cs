using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

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
        public virtual void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        { 
        
        }
    }
}
