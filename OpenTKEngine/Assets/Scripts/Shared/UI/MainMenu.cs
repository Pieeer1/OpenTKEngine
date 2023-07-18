using ImGuiNET;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Services;
using OpenTK.Mathematics;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Assets.Scripts.Shared.UI
{
    public class MainMenu : SharedUIElementWithOptions
    {
        private Label _titleTextReference;
        private Button _startServerButtonReference;
        private Button _joinServerButtonReference;
        private Button _optionsButton;
        private Button _quitButton;
        private OptionsMenu _optionsMenu;
        private StartServerMenu _startServerMenu;
        public MainMenu(CanvasComponent canvas) : base(canvas)
        {
            const ImGuiWindowFlags baseFlags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoBackground;

            Vector2 buttonSize = new Vector2(250, 50);

            _titleTextReference = new Label("Untitled Game", baseFlags, "window0", Styles.ColorDictionary["DefaultText"], (WindowService.Instance.ScreenSize.ToVector2() / 2) - buttonSize/2); // todo change size to not be directly in center
            _startServerButtonReference = new Button(PressStartServer, "Start Server", buttonSize, baseFlags, "window0");
            _joinServerButtonReference = new Button(PressJoinServer, "Join Server", buttonSize, baseFlags, "window0");
            _optionsButton = new Button(PressOptionsButton, "Options", buttonSize, baseFlags, "window0");
            _quitButton = new Button(PressQuitButton, "Quit", buttonSize, baseFlags, "window0");

            _canvas.AddUIElement(_titleTextReference);
            _canvas.AddUIElement(_startServerButtonReference);
            _canvas.AddUIElement(_joinServerButtonReference);
            _canvas.AddUIElement(_optionsButton);
            _canvas.AddUIElement(_quitButton);

            _optionsMenu = new OptionsMenu(canvas, this);
            _startServerMenu = new StartServerMenu(canvas, this);

            EnableDisableUIElements(true);
        }

        public override void EnableDisableUIElements(bool val)
        {
            _titleTextReference.IsActive = val;
            _startServerButtonReference.IsActive = val;
            _joinServerButtonReference.IsActive = val;
            _optionsButton.IsActive = val;
            _quitButton.IsActive = val;
        }
        private void PressStartServer()
        {
            EnableDisableUIElements(false);
            _startServerMenu.EnableDisableUIElements(true);
        }
        private void PressJoinServer()
        { 
        
        }
        private void PressOptionsButton() 
        {
            InputFlagService.Instance.ActiveInputFlags = Enums.InputFlags.Options;

            EnableDisableUIElements(false);
            _optionsMenu.EnableDisableUIElements(true);
        }
        private void PressQuitButton() 
        {
            WindowService.Instance.GameWindowReference.Close();
        }
    }
}
