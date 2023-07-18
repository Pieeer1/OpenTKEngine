using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Services;

namespace OpenTKEngine.Assets.Scripts.Shared.UI
{
    public class PauseMenu : SharedUIElementWithOptions
    {
        private Label _labelReference;
        private Button _optionsButtonReference;
        private Button _quitButtonReference;
        private OptionsMenu _optionsMenu;
        public PauseMenu(CanvasComponent canvas) : base(canvas)
        {
            const ImGuiWindowFlags baseFlags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize;

            Vector2 buttonSize = new Vector2(250, 50);

            _labelReference = new Label("Menu", baseFlags, "window0", location: (WindowService.Instance.ScreenSize/2) - (buttonSize/2),toggleKey: Keys.Escape);
            _optionsButtonReference = new Button(PressOptions, "Options", buttonSize, baseFlags, "window0", toggleKey: Keys.Escape);
            _quitButtonReference = new Button(WindowService.Instance.GameWindowReference.Close, "Quit", buttonSize, baseFlags, "window0", toggleKey: Keys.Escape);
            _canvas.AddUIElement(_labelReference);
            _canvas.AddUIElement(_optionsButtonReference);
            _canvas.AddUIElement(_quitButtonReference);

            _optionsMenu = new OptionsMenu(canvas, this);

            EnableDisableUIElements(false);

            _optionsButtonReference.OnToggle += Menu_OnToggle;
        }

        public override void EnableDisableUIElements(bool val)
        {
            _labelReference.IsActive = val;
            _optionsButtonReference.IsActive = val;
            _quitButtonReference.IsActive = val;
        }

        private void Menu_OnToggle(object? sender, EventArgs e)
        {
            if (SentFromOptions)
            {
                SentFromOptions = false;
                return;
            }
            if ((InputFlagService.Instance.ActiveInputFlags & Enums.InputFlags.Menu) == 0) { return; }

            EnableDisableUIElements(!_labelReference.IsActive);

            if (_optionsButtonReference.IsActive)
            {
                WindowService.Instance.ActiveCursorState = OpenTK.Windowing.Common.CursorState.Normal;
                InputFlagService.Instance.ActiveInputFlags &= Enums.InputFlags.Menu;
            }
            else
            {
                WindowService.Instance.ActiveCursorState = OpenTK.Windowing.Common.CursorState.Grabbed;
                InputFlagService.Instance.ActiveInputFlags |= Enums.InputFlags.Reset;
            }
        }
        private void PressOptions()
        {
            InputFlagService.Instance.ActiveInputFlags = Enums.InputFlags.Options;

            EnableDisableUIElements(false);
            _optionsMenu.EnableDisableUIElements(true);
        }
    }
}
