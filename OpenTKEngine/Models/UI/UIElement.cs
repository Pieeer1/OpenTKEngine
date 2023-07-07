using ImGuiNET;

namespace OpenTKEngine.Models.UI
{
    public abstract class UIElement
    {
        private protected ImGuiWindowFlags _imGuiWindowFlags;
        private protected string _name;

        protected UIElement(ImGuiWindowFlags imGuiWindowFlags, string name)
        {
            _imGuiWindowFlags = imGuiWindowFlags;
            _name = name;
        }

        public virtual void StartRender()
        {
            ImGui.Begin(_name, _imGuiWindowFlags);
        }
        public virtual void EndRender()
        {
            ImGui.End();
        }
    }
}
