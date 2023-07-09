using ImGuiNET;
using OpenTK.Mathematics;

namespace OpenTKEngine.Models.UI
{
    public abstract class UIElement
    {
        private protected ImGuiWindowFlags _imGuiWindowFlags;
        private protected string _name;
        private protected Vector2? _location;
        protected UIElement(ImGuiWindowFlags imGuiWindowFlags, string name, Vector2? location = null)
        {
            _imGuiWindowFlags = imGuiWindowFlags;
            _name = name;
            _location = location;
        }

        public virtual void StartRender()
        {
            ImGui.Begin(_name, _imGuiWindowFlags);
            if (_location is not null)
            { 
                ImGui.SetCursorPos(new System.Numerics.Vector2(_location.Value.X, _location.Value.Y));
            }
        }
        public virtual void EndRender()
        {
            ImGui.End();
        }
    }
}
