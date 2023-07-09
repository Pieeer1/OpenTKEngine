using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKEngine.Models.UI
{
    public abstract class UIElement
    {
        private protected ImGuiWindowFlags _imGuiWindowFlags;
        private protected string _name;
        private protected Vector2? _location;
        public Keys ToggleKey {get; set;}
        public bool IsActive { get; set; }
        protected UIElement(ImGuiWindowFlags imGuiWindowFlags, string name, Vector2? location = null, Keys toggleKey = Keys.Unknown)
        {
            _imGuiWindowFlags = imGuiWindowFlags;
            _name = name;
            _location = location;
            ToggleKey = toggleKey;
        }
        public event EventHandler? OnToggle;
        public virtual void StartRender()
        {
            if (_location is not null)
            {
                ImGui.SetNextWindowPos(new System.Numerics.Vector2(_location.Value.X, _location.Value.Y));
            }
            ImGui.Begin(_name, _imGuiWindowFlags);
        }
        public virtual void EndRender()
        {
            ImGui.End();
        }

        public void Toggle()
        {
            if (OnToggle is not null)
            { 
                OnToggle.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
