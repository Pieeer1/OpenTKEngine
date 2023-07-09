using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Enums;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.Shared.UI
{
    public class ChatInput : SharedUIElement
    {
        private TextBox _textBoxReference;
        public ChatInput(CanvasComponent canvas) : base(canvas)
        {
            ImGuiWindowFlags flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoBackground;
            _textBoxReference = new TextBox("Chat", OnTextChange, flags, "chatWindow", new Vector2(15.0f, 500.0f), Keys.T, true);
            _canvas.AddUIElement(_textBoxReference);

            _textBoxReference.OnToggle += TextBoxRef_OnToggle;
        }

        private void TextBoxRef_OnToggle(object? sender, EventArgs e)
        {
            if ((InputFlagService.Instance.ActiveInputFlags & InputFlags.Chat) == 0) { return; }

            _textBoxReference.IsActive = !_textBoxReference.IsActive;

            if (_textBoxReference.IsActive)
            {
                _textBoxReference.ToggleKey = Keys.Escape;

                InputFlagService.Instance.ActiveInputFlags &= InputFlags.Chat;
            }
            else
            {
                _textBoxReference.ToggleKey = Keys.T;

                InputFlagService.Instance.ActiveInputFlags |= InputFlags.All;
            }
        }

        public void OnTextChange(string s)
        {

        }
    }
}
