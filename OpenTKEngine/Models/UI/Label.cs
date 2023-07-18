using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Models.UI
{
    public class Label : UIElement
    {
        public string Text { get; set; }
        public Vector4 Color { get; set; }
        public Label(string text, ImGuiWindowFlags imGuiWindowFlags, string name, Vector4? color = null, Vector2? location = null, Keys toggleKey = Keys.Unknown) : base(imGuiWindowFlags, name, location, toggleKey)
        {
            Text = text;
            Color = color ?? Styles.ColorDictionary["DefaultText"];
        }

        public override void StartRender()
        {
            base.StartRender();
            Vector4 color = Color;
            ImGui.TextColored(new System.Numerics.Vector4(color.X, color.Y, color.Z, color.W), Text);        
        }
    }
}
