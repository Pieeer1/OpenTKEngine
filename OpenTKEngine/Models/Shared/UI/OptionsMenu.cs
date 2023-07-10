using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Services;
using static OpenTKEngine.Models.Constants;
namespace OpenTKEngine.Models.Shared.UI
{
    public class OptionsMenu : SharedUIElement
    {
        private Checkbox _fullscreenCheckboxReference;
        private DropdownMenu _resolutionDropdownReference;
        private Button _backButtonReference;
        private Button _saveButtonReference;
        private Menu _menuRef;
        private Dictionary<string, Action> commandQueue = new Dictionary<string, Action>();
        public OptionsMenu(CanvasComponent canvas, Menu menuRef) : base(canvas)
        {
            const ImGuiWindowFlags baseFlags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize;
            _backButtonReference = new Button(BackPress, "Back", new Vector2(250, 50), baseFlags, "optionsWindow");
            _saveButtonReference = new Button(SavePress, "Save", new Vector2(250, 50), baseFlags, "optionsWindow");
            _resolutionDropdownReference = new DropdownMenu(ParseResolution(WindowService.Instance.ScreenSize), LimitedLists.Resolutions, baseFlags, "optionsWindow", "Resolution");
            _fullscreenCheckboxReference = new Checkbox(baseFlags, "optionsWindow", "Is Fullscreen?");
            _canvas.AddUIElement(_fullscreenCheckboxReference);
            _canvas.AddUIElement(_resolutionDropdownReference);
            _canvas.AddUIElement(_saveButtonReference);
            _canvas.AddUIElement(_backButtonReference);

            _menuRef = menuRef;

            EnableDisableUIElements(false);

            _resolutionDropdownReference.OnNewSelect += ResolutionDropdownReference_OnNewSelect;
            _fullscreenCheckboxReference.OnChecked += FullscreenCheckboxReference_OnChecked;
            _canvas.OnComponentKeyInput += Canvas_OnComponentKeyInput;
        }

        private void ResolutionDropdownReference_OnNewSelect(object? sender, DropdownArgs e)
        {
            if (!commandQueue.TryAdd(nameof(ResolutionDropdownReference_OnNewSelect), () =>
            {
                WindowService.Instance.ScreenSize = ParseResolution(e.SelectedValue);
                
            }))
            {
                commandQueue[nameof(ResolutionDropdownReference_OnNewSelect)] = () =>
                { 
                    WindowService.Instance.ScreenSize = ParseResolution(e.SelectedValue);
                };
            }
        }

        private void FullscreenCheckboxReference_OnChecked(object? sender, CheckboxArgs e)
        {
            if (!commandQueue.TryAdd(nameof(FullscreenCheckboxReference_OnChecked), () => WindowService.Instance.WindowState = e.IsChecked ? WindowState.Fullscreen : WindowState.Normal))
            {
                commandQueue[nameof(FullscreenCheckboxReference_OnChecked)] = () => WindowService.Instance.WindowState = e.IsChecked ? WindowState.Fullscreen : WindowState.Normal;
            }
        }
        private void SavePress()
        {
            foreach (Action action in commandQueue.Values)
            {
                action.Invoke();
            }
            commandQueue.Clear();
        }
        private void Canvas_OnComponentKeyInput(object? sender, Entities.ComponentEventArgs e)
        {
            if (_backButtonReference.IsActive && e.KeyboardState.IsKeyPressed(Keys.Escape))
            {
                _menuRef.SentFromOptions = true;
                SwapBackToMenu();
            }
        }

        public override void EnableDisableUIElements(bool val)
        {
            _backButtonReference.IsActive = val;
            _fullscreenCheckboxReference.IsActive = val;
            _saveButtonReference.IsActive = val;
            _resolutionDropdownReference.IsActive = val;
        }

        private void BackPress()
        {
            SwapBackToMenu();
        }


        private void SwapBackToMenu()
        {
            InputFlagService.Instance.ActiveInputFlags = Enums.InputFlags.Menu;
            _fullscreenCheckboxReference.IsChecked = WindowService.Instance.WindowState == WindowState.Fullscreen;
            EnableDisableUIElements(false);
            _menuRef.EnableDisableUIElements(true);
        }

        private string ParseResolution(Vector2i resolution) => $"{resolution.X}x{resolution.Y}";
        private Vector2i ParseResolution(string resolution) => new Vector2i(int.Parse(resolution.Split('x')[0]), int.Parse(resolution.Split('x')[1]));
    }
}
