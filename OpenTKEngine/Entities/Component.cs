using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Enums;
using OpenTKEngine.Scenes;
using OpenTKEngine.Services;

namespace OpenTKEngine.Entities
{
    public class Component
    {
        public Entity Entity { get; set; } = null!;
        public EntityComponentManager EntityComponentManager { get => SceneManager.Instance.ActiveScene.EntityComponentManager; }
        protected InputFlags ActiveInputFlags { get => InputFlagService.Instance.ActiveInputFlags; set => InputFlagService.Instance.ActiveInputFlags = value; }
        public double DeltaTime { get => TimeService.Instance.DeltaTime; }

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
