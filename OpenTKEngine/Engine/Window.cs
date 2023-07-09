using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Attributes;
using OpenTKEngine.Enums;
using OpenTKEngine.Scenes;
using OpenTKEngine.Services;
using System.Diagnostics;

namespace OpenTKEngine.Engine
{
    public class Window : GameWindow
    {
        private bool _firstMove = true;

        private int _frameCount;
        private Vector2 _lastPos;

        private readonly SceneManager _sceneManager;
        private readonly TimeService _timeService;
        private readonly InputFlagService _inputFlagService;
        private readonly CursorService _cursorService;
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            _sceneManager = SceneManager.Instance;
            _timeService = TimeService.Instance;
            _inputFlagService = InputFlagService.Instance;
            _cursorService = CursorService.Instance;
            _cursorService.GameWindowReference = this; 
        }
        protected override void OnLoad()
        {
            base.OnLoad();

            _sceneManager.AddScene(new BaseDebugScene("base debug 1")); // default scene
            _sceneManager.SwapScene(0);
            _sceneManager.LoadScene(0);

            _cursorService.ActiveCursorState = CursorState.Grabbed;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            _sceneManager.DrawActiveScene();

            SwapBuffers();
            
            _sceneManager.RefreshActiveScene();

            _frameCount++;
            _sceneManager.SetActiveComponentReferences(new OnTickAttribute(), _frameCount);

            _timeService.DeltaTime = RenderTime + UpdateTime;
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            _sceneManager.UpdateActiveScene();

            if (!IsFocused)
            {
                return;
            }

            _sceneManager.UpdateActiveInput(e, KeyboardState, MouseState, ref _firstMove, ref _lastPos);

            if (KeyboardState.IsKeyPressed(Keys.Escape))
            {
                //CursorState = CursorState == CursorState.Grabbed ? CursorState.Normal : CursorState.Grabbed;
                //if (CursorState != CursorState.Grabbed)
                //{
                //    _inputFlagService.ActiveInputFlags &= InputFlags.Menu;
                //}
                //else
                //{
                //    _inputFlagService.ActiveInputFlags |= InputFlags.Menu | InputFlags.Player;
                //}
            }
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            base.OnTextInput(e);
            _sceneManager.SetActiveComponentReferences(new CharacterPressAttribute(), (char)e.Unicode);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            //fov tings
            //_sceneManager.SetActiveComponentReferences(new OnMouseWheelAttribute(), e.OffsetY);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);

            HandleResizeAttributes(Size);
        }
        private void HandleResizeAttributes(Vector2 screenSize)
        {
            _sceneManager.SetActiveComponentReferences(new OnResizeAttribute(), screenSize);
        }
    }
}
