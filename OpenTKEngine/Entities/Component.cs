using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Enums;
using OpenTKEngine.Scenes;
using OpenTKEngine.Services;
using System.Text.Json.Serialization;

namespace OpenTKEngine.Entities
{
    public class Component
    {
        public bool IsEnabled { get; set; } = true;

        private Entity _entity = null!;
        [JsonIgnore]
        public Entity Entity { get => _entity; internal set 
            {
                if(value is null) { return; }
                _entity ??= value;
            } 
        }
        public EntityComponentManager EntityComponentManager { get => SceneManager.Instance.ActiveScene.EntityComponentManager; }
        protected InputFlags ActiveInputFlags { get => InputFlagService.Instance.ActiveInputFlags; set => InputFlagService.Instance.ActiveInputFlags = value; }
        public static double DeltaTime { get => TimeService.Instance.DeltaTime; }
        public event EventHandler<ComponentEventArgs>? OnComponentKeyInput;
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
            ComponentKeyInput(input);
        }
        protected void ComponentKeyInput(KeyboardState keyboardState)
        {
            if (OnComponentKeyInput is not null)
            {
                OnComponentKeyInput.Invoke(this, new ComponentEventArgs(keyboardState));
            }
        }
    }
    public class ComponentEventArgs : EventArgs
    {
        public ComponentEventArgs(KeyboardState keyboardState)
        {
            KeyboardState = keyboardState;
        }

        public KeyboardState KeyboardState { get; private set; }
    }
}
