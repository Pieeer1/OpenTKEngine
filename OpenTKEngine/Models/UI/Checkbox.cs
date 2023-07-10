using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

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
