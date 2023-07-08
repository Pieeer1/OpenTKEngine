using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities;
using OpenTKEngine.Models;
using OpenTK.Mathematics;
using OpenTKEngine.Services;

namespace OpenTKEngine.Scenes
{
    public abstract class Scene
    {
        protected Dictionary<string, Shader> _shaders = new Dictionary<string, Shader>();
        public EntityComponentManager EntityComponentManager { get; private set; } = new EntityComponentManager();

        public Scene(string name)
        {
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsLoaded { get; set; } = false;
        public void OnLoad()
        {
            if (IsLoaded) { return; }
            OnAwake();
            IsLoaded = true;
        }
        public abstract void OnAwake();
        public virtual void OnDraw()
        {
            EntityComponentManager.Draw();
        }
        public virtual void OnUpdate()
        {
            EntityComponentManager.Update();
        }
        public virtual void Refresh()
        {
            EntityComponentManager.Refresh();
        }
        public virtual void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            EntityComponentManager.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
        }
        public virtual void SetComponentReferencesWithAttribute(Attribute att, object value)
        {
            EntityComponentManager.SetComponentReferencesWithAttribute(att, value);
        }
    }
}
