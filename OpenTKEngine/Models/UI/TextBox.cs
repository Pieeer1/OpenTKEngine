using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.UI
{
    public class TextBox : UIElement
    {
        private string _defaultText;
        private Action<string> _action;
        private string _inputString = string.Empty;
        private bool _setFocusWhileActive;
        public TextBox(string defaultText, Action<string> action, ImGuiWindowFlags imGuiWindowFlags, string name, Vector2? location = null, Keys toggleKey = Keys.Unknown, bool setFocusWhileActive = false) : base(imGuiWindowFlags, name, location, toggleKey)
        {
            _defaultText = defaultText;
            _action = action;
            _setFocusWhileActive = setFocusWhileActive;
        }
        public override void StartRender()
        {
            base.StartRender();
            if (_setFocusWhileActive)
            { 
                ImGui.SetKeyboardFocusHere();
            }
            if (ImGui.InputTextWithHint(string.Empty, _defaultText, ref _inputString, 256))
            { 
                _action.Invoke(_inputString);
            };
        }
    }
}
