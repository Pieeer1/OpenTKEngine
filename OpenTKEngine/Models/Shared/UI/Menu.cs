using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.Shared.UI
{
    public class Menu : SharedUIElement
    {
        private readonly Label _labelReference;
        private readonly Button _optionsButtonReference;
        private readonly Button _quitButtonReference;
        public Menu(CanvasComponent canvas) : base(canvas)
        {
            const ImGuiWindowFlags baseFlags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize;

            _labelReference = new Label("Menu", baseFlags, "window0", toggleKey: Keys.Escape);
            _optionsButtonReference = new Button(() => Console.WriteLine("Options"), "Options", new Vector2(250, 50), baseFlags, "menuWindow", toggleKey: Keys.Escape);
            _quitButtonReference = new Button(() => Environment.Exit(0), "Quit", new Vector2(250, 50), baseFlags, "menuWindow", toggleKey: Keys.Escape);
            canvas.AddUIElement(_labelReference);
            canvas.AddUIElement(_optionsButtonReference);
            canvas.AddUIElement(_quitButtonReference);

            _labelReference.IsActive = false;
            _optionsButtonReference.IsActive = false;
            _quitButtonReference.IsActive = false;

            _optionsButtonReference.OnToggle += Menu_OnToggle;
        }

        private void Menu_OnToggle(object? sender, EventArgs e)
        {
            if ((InputFlagService.Instance.ActiveInputFlags & Enums.InputFlags.Menu) == 0) { return; }

            _labelReference.IsActive = !_labelReference.IsActive;
            _optionsButtonReference.IsActive = !_optionsButtonReference.IsActive;
            _quitButtonReference.IsActive = !_quitButtonReference.IsActive;

            if (_optionsButtonReference.IsActive)
            {
                CursorService.Instance.ActiveCursorState = OpenTK.Windowing.Common.CursorState.Normal;
                InputFlagService.Instance.ActiveInputFlags &= Enums.InputFlags.Menu;
            }
            else
            { 
                CursorService.Instance.ActiveCursorState = OpenTK.Windowing.Common.CursorState.Grabbed;
                InputFlagService.Instance.ActiveInputFlags |= Enums.InputFlags.All;

            }
        }
    }
}
