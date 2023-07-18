using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Services;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Assets.Scripts.Shared.UI
{
    public class StartServerMenu : SharedUIElement
    {
        private Label _startServerLabel;
        private Button _backButtonReference;

        private MainMenu _mainMenuReference;

        public StartServerMenu(CanvasComponent canvas, MainMenu mainMenuRef) : base(canvas)
        {
            const ImGuiWindowFlags baseFlags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoBackground;

            Vector2 buttonSize = new Vector2(250, 50);

            _startServerLabel = new Label("Start Sever", baseFlags, "window0", Styles.ColorDictionary["DefaultText"], (WindowService.Instance.ScreenSize.ToVector2() / 2) - buttonSize / 2);
            _backButtonReference = new Button(PressBackButton, "Back", buttonSize, baseFlags, "window0");


            _canvas.AddUIElement(_startServerLabel);
            _canvas.AddUIElement(_backButtonReference);

            _canvas.OnComponentKeyInput += Canvas_OnComponentKeyInput;

            _mainMenuReference = mainMenuRef;
        }

        private void Canvas_OnComponentKeyInput(object? sender, OpenTKEngine.Entities.ComponentEventArgs e)
        {
            if (_backButtonReference.IsActive && e.KeyboardState.IsKeyPressed(Keys.Escape))
            {
                PressBackButton();
            }
        }

        private void PressBackButton()
        {
            EnableDisableUIElements(false);
            _mainMenuReference.EnableDisableUIElements(true);

        }

        public override void EnableDisableUIElements(bool val)
        {
            _startServerLabel.IsActive = val;
            _backButtonReference.IsActive = val;
        }
    }
}
