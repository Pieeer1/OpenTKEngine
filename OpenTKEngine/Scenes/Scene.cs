using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities;
using OpenTKEngine.Models;
using OpenTK.Mathematics;

namespace OpenTKEngine.Scenes
{
    public abstract class Scene
    {
        protected Dictionary<string, Shader> _shaders = new Dictionary<string, Shader>();
        protected abstract EntityComponentManager _entityComponentManager { get; }

        public Scene(string name)
        {
            Name = name;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = false;

        public abstract void OnLoad();
        public virtual void OnDraw()
        {
            _entityComponentManager.Draw();
        }
        public virtual void OnUpdate()
        { 
            _entityComponentManager.Update();
        }
        public virtual void Refresh()
        { 
            _entityComponentManager.Refresh();
        }
        public virtual void UpdateInput(FrameEventArgs e, KeyboardState input, MouseState mouse, ref bool firstMove, ref Vector2 lastPos)
        {
            _entityComponentManager.UpdateInput(e, input, mouse, ref firstMove, ref lastPos);
        }
        public virtual void SetComponentReferencesWithAttribute(Attribute att, object value)
        {
            _entityComponentManager.SetComponentReferencesWithAttribute(att, value);
        }
    }
}
