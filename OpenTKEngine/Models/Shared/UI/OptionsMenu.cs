﻿using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.Shared.UI
{
    public class OptionsMenu : SharedUIElement
    {
        private Button _backButtonReference;
        private Menu _menuRef;
        public OptionsMenu(CanvasComponent canvas, Menu menuRef) : base(canvas)
        {
            const ImGuiWindowFlags baseFlags = ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.AlwaysAutoResize;
            _backButtonReference = new Button(BackPress, "Back", new Vector2(250, 50), baseFlags, "optionsWindow", toggleKey: Keys.Escape);
            _canvas.AddUIElement(_backButtonReference);

            _menuRef = menuRef;

            EnableDisableUIElements(false);

            _canvas.OnComponentKeyInput += Canvas_OnComponentKeyInput;
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
        }

        private void BackPress()
        {
            SwapBackToMenu();
        }


        private void SwapBackToMenu()
        {
            InputFlagService.Instance.ActiveInputFlags = Enums.InputFlags.Menu;

            EnableDisableUIElements(false);
            _menuRef.EnableDisableUIElements(true);
        }

    }
}
