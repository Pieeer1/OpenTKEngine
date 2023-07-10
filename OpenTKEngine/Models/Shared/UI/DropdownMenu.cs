using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Models.UI;

namespace OpenTKEngine.Models.Shared.UI
{
    public class DropdownMenu : UIElement
    {
        private string _defaultOption;
        private string _label;
        private List<string> _options;

        public DropdownMenu(string defaultOption, IEnumerable<string> options, ImGuiWindowFlags imGuiWindowFlags, string name, string label, Vector2? location = null, Keys toggleKey = Keys.Unknown) : base(imGuiWindowFlags, name, location, toggleKey)
        {
            _defaultOption = defaultOption;
            _options = options.ToList();
            _label = label;
        }


        public override void StartRender()
        {
            base.StartRender();
            ImGui.BeginCombo(_label, _defaultOption);
            foreach (string option in _options)
            {
                bool isSelected = _defaultOption == option;
                if (ImGui.MenuItem(option, string.Empty, isSelected))
                {
                    _defaultOption = option;
                }
            }
        }
        public override void EndRender()
        {
            ImGui.EndCombo();
            base.EndRender();
        }
    }
}
