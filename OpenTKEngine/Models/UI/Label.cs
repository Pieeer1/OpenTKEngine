using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKEngine.Models.UI
{
    public class Label : UIElement
    {
        private readonly string _text;

        public Label(string text, ImGuiWindowFlags imGuiWindowFlags, string name, Vector2? location = null, Keys toggleKey = Keys.Unknown) : base(imGuiWindowFlags, name, location, toggleKey)
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
