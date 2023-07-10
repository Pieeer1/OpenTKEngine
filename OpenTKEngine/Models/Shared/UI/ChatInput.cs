using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Enums;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Services;
using System.Text;

namespace OpenTKEngine.Models.Shared.UI
{
    public class ChatInput : SharedUIElement
    {
        private TextBox _textBoxReference;
        private string _chat = string.Empty;
        public ChatInput(CanvasComponent canvas) : base(canvas)
        {
            ImGuiWindowFlags flags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoBackground;
            _textBoxReference = new TextBox("Chat", OnTextChange, flags, "chatWindow", new Vector2(15.0f, 500.0f), Keys.T, true);
            _canvas.AddUIElement(_textBoxReference);

            _textBoxReference.OnToggle += TextBoxRef_OnToggle;
            _canvas.OnComponentKeyInput += Canvas_OnComponentKeyInput;
        }

        private void Canvas_OnComponentKeyInput(object? sender, Entities.ComponentEventArgs e)
        {
            if (_textBoxReference.IsActive && e.KeyboardState.IsKeyPressed(Keys.Enter) && !string.IsNullOrWhiteSpace(_chat))
            {
                MessageService.Instance.LogChat(_chat);
                _textBoxReference.Clear();
            }
        }

        private void TextBoxRef_OnToggle(object? sender, EventArgs e)
        {
            if ((InputFlagService.Instance.ActiveInputFlags & InputFlags.Chat) == 0) { return; }

            EnableDisableUIElements(!_textBoxReference.IsActive);

            if (_textBoxReference.IsActive)
            {
                _textBoxReference.ToggleKey = Keys.Escape;
                WindowService.Instance.ActiveCursorState = OpenTK.Windowing.Common.CursorState.Normal;
                InputFlagService.Instance.ActiveInputFlags &= InputFlags.Chat;
            }
            else
            {
                _textBoxReference.ToggleKey = Keys.T;
                WindowService.Instance.ActiveCursorState = OpenTK.Windowing.Common.CursorState.Grabbed;
                InputFlagService.Instance.ActiveInputFlags |= InputFlags.Reset;
            }
        }

        public void OnTextChange(string s)
        {
            _chat = s;
        }

        public override void EnableDisableUIElements(bool val)
        {
            _textBoxReference.IsActive = val;
        }
    }
}
