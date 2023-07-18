using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenTKEngine.Models.UI
{
    public class TextBox : UIElement
    {
        private string _defaultText;
        private Action<string> _action;
        private string _inputString = string.Empty;
        private bool _setFocusWhileActive;
        private bool _shouldClear;
        private int _uuid;
        public TextBox(string defaultText, Action<string> action, ImGuiWindowFlags imGuiWindowFlags, string name, Vector2? location = null, Keys toggleKey = Keys.Unknown, bool setFocusWhileActive = false, int uuid = 0) : base(imGuiWindowFlags, name, location, toggleKey)
        {
            _defaultText = defaultText;
            _action = action;
            _setFocusWhileActive = setFocusWhileActive;
            _uuid = uuid;
        }
        public override void StartRender()
        {
            base.StartRender();
            if (_setFocusWhileActive)
            {
                ImGui.SetKeyboardFocusHere();
            }
            ImGui.PushID(_uuid);
            if (ImGui.InputTextWithHint(string.Empty, _defaultText, ref _inputString, 256))
            { 
                _action.Invoke(_inputString);
            };
            ImGui.PopID();

            if (_shouldClear) // I shit you not this is how you have to implement it due to the rendering timing, you cannot just clear the field lol
            {
                _shouldClear = !_shouldClear;
                _inputString = string.Empty;
            }
        }
        public void Clear()
        {
            _shouldClear = true;
        }
    }
}
