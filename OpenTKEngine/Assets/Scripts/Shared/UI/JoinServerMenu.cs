using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Services;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Assets.Scripts.Shared.UI
{
    public class JoinServerMenu : SharedUIElement
    {
        private Label _joinServerLabel;
        private TextBox _addressTextBoxReference;
        private TextBox _portTextBoxReference;
        private Button _backButtonReference;
        private Button _joinServerButtonReference;

        private string _address = "localhost";
        private ushort _port = 1234;

        private readonly MainMenu _mainMenuReference;
        public JoinServerMenu(CanvasComponent canvas, MainMenu mainMenuRef) : base(canvas)
        {
            const ImGuiWindowFlags baseFlags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoBackground;

            Vector2 buttonSize = new Vector2(250, 50);

            _joinServerLabel = new Label("Start Sever", baseFlags, "window0", Styles.ColorDictionary["DefaultText"], (WindowService.Instance.ScreenSize.ToVector2() / 2) - buttonSize / 2);
            _joinServerButtonReference = new Button(PressJoinButton, "Start", buttonSize, baseFlags, "window0");
            _addressTextBoxReference = new TextBox("Address", OnAddressTextChange, baseFlags, "window0", uuid: 0);
            _portTextBoxReference = new TextBox("Port", OnPortTextChange, baseFlags, "window0", uuid: 1);
            _backButtonReference = new Button(PressBackButton, "Back", buttonSize, baseFlags, "window0");

            _canvas.AddUIElement(_joinServerLabel);
            _canvas.AddUIElement(_joinServerButtonReference);
            _canvas.AddUIElement(_addressTextBoxReference);
            _canvas.AddUIElement(_portTextBoxReference);
            _canvas.AddUIElement(_backButtonReference);

            _canvas.OnComponentKeyInput += Canvas_OnComponentKeyInput;


            _mainMenuReference = mainMenuRef;
        }

        private void PressJoinButton()
        {
            // load scene
            // start client
            // attempt to join server
        }

        private void OnPortTextChange(string port)
        {
            if (!ushort.TryParse(port, out _port))
            {
                _port = 1234;
            }
        }

        private void OnAddressTextChange(string address)
        {
            _address = address;
        }

        private void Canvas_OnComponentKeyInput(object? sender, ComponentEventArgs e)
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
            _joinServerLabel.IsActive = val;
            _joinServerButtonReference.IsActive = val;
            _addressTextBoxReference.IsActive = val;
            _portTextBoxReference.IsActive = val;
            _backButtonReference.IsActive = val;
        }
    }
}
