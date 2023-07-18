using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Services;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Models.UI
{
    public class Checkbox : UIElement
    {
        private readonly string? _label;
        public bool IsChecked { get; set; }
        public Checkbox(ImGuiWindowFlags imGuiWindowFlags, string name, string? label = null, Vector2? location = null, Keys toggleKey = Keys.Unknown) : base(imGuiWindowFlags, name, location, toggleKey)
        {
            _label = label;
        }
        public event EventHandler<CheckboxArgs>? OnChecked;
        public override void StartRender()
        {
            base.StartRender();
            bool referenceableCheck = IsChecked;
            ImGuiStylePtr style = ImGui.GetStyle();
            style.Colors[(int)ImGuiCol.CheckMark] = DataManipulationService.OpenTKVectorToSystemVector(Styles.ColorDictionary["SpecialText"]); // Set the checkmark color
            style.Colors[(int)ImGuiCol.FrameBg] = DataManipulationService.OpenTKVectorToSystemVector(Styles.ColorDictionary["BackgroundColor"]); // Set the frame background color
            style.Colors[(int)ImGuiCol.FrameBgHovered] = DataManipulationService.OpenTKVectorToSystemVector(Styles.ColorDictionary["AccentColor1"]); // Set the frame background color when hovered
            style.Colors[(int)ImGuiCol.FrameBgActive] = DataManipulationService.OpenTKVectorToSystemVector(Styles.ColorDictionary["AccentColor2"]); // Set the frame background color when active

            if (ImGui.Checkbox(_label ?? string.Empty, ref referenceableCheck))
            {
                IsChecked = !IsChecked;
                Checked(IsChecked);
            }
        }
        public void Checked(bool val)
        {
            if (OnChecked is not null)
            {
                OnChecked.Invoke(this, new CheckboxArgs(val));
            }
        }
    }
    public class CheckboxArgs : EventArgs
    {
        public CheckboxArgs(bool isChecked)
        {
            IsChecked = isChecked;
        }

        public bool IsChecked { get; set; }
    }
}
