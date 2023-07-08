using ImGuiNET;
using OpenTK.Mathematics;

namespace OpenTKEngine.Models.UI
{
    public class TextBox : UIElement
    {
        private string _defaultText;
        private Action<string> _action;
        private string _inputString = string.Empty;
        public TextBox(string defaultText, Action<string> action, ImGuiWindowFlags imGuiWindowFlags, string name, Vector2? location = null) : base(imGuiWindowFlags, name, location)
        {
            _defaultText = defaultText;
            _action = action;
        }
        public override void StartRender()
        {
            base.StartRender();
            ImGui.InputTextWithHint(string.Empty, _defaultText, ref _inputString, 60);
            _action.Invoke(_inputString);
        }
    }
}
