using ImGuiNET;
using OpenTK.Mathematics;

namespace OpenTKEngine.Models.UI
{
    public class Label : UIElement
    {
        private string _text;

        public Label(string text, ImGuiWindowFlags imGuiWindowFlags, string name, Vector2? location = null) : base(imGuiWindowFlags, name, location)
        {
            _text = text;
        }

        public override void StartRender()
        {
            base.StartRender();
            ImGui.Text(_text);
        }
    }
}
