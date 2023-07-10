using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKEngine.Models.UI
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
            if (!_options.Any(x => x == _defaultOption))
            {
                _options.Add(_defaultOption);
            }
            _label = label;
        }

        public event EventHandler<DropdownArgs>? OnNewSelect;
        public override void StartRender()
        {
            base.StartRender();
            int optionLocation = _options.IndexOf(_defaultOption);
            if (ImGui.ListBox(_label, ref optionLocation, _options.ToArray(), _options.Count))
            {
                _defaultOption = _options[optionLocation];
                NewSelect(_defaultOption);
            };
        }

        public void NewSelect(string value)
        {
            if (OnNewSelect is not null)
            {
                OnNewSelect.Invoke(this, new DropdownArgs(value));
            }
        }
    }
    public class DropdownArgs : EventArgs
    {
        public DropdownArgs(string selectedValue)
        {
            SelectedValue = selectedValue;
        }

        public string SelectedValue { get; set; }
    }
}
