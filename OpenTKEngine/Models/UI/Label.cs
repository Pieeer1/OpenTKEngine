using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKEngine.Models.UI
{
    public class Label : UIElement
    {
        public string Text { get; set; }
        public Vector4? Color { get; set; }
        public Label(string text, ImGuiWindowFlags imGuiWindowFlags, string name, Vector4? color = null, Vector2? location = null, Keys toggleKey = Keys.Unknown) : base(imGuiWindowFlags, name, location, toggleKey)
        {
            Text = text;
            Color = color;
        }

        public override void StartRender()
        {
            base.StartRender();
            if (Color is null)
            { 
                ImGui.Text(Text);
            }
            else
            {
                Vector4 color = Color.Value;
                ImGui.TextColored(new System.Numerics.Vector4(color.X, color.Y, color.Z, color.W), Text);
            }
        }
    }
}
