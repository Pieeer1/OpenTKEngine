using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Attributes;
using OpenTKEngine.Scenes;
using OpenTKEngine.Services;

namespace OpenTKEngine.Engine
{
    public class Window : GameWindow
    {
        private bool _firstMove = true;

        private Vector2 _lastPos;

        private readonly SceneManager _sceneManager;
        private readonly TimeService _timeService;
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
        {
            _sceneManager = SceneManager.Instance;
            _timeService = TimeService.Instance;
        }
        protected override void OnLoad()
        {
            base.OnLoad();

            _sceneManager.AddScene(new BaseDebugScene("base debug 1")); // default scene
            _sceneManager.SwapScene(0);
            _sceneManager.LoadScene(0);

            CursorState = CursorState.Grabbed;
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            _sceneManager.DrawActiveScene();

            SwapBuffers();
            
            _sceneManager.RefreshActiveScene();
            _timeService.DeltaTime = RenderTime + UpdateTime;
        }

        private int _frameCount;
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            _frameCount++;

            _sceneManager.UpdateActiveScene();

            if (!IsFocused)
            {
                return;
            }

            _sceneManager.UpdateActiveInput(e, KeyboardState, MouseState, ref _firstMove, ref _lastPos);

            _sceneManager.SetActiveComponentReferences(new OnTickAttribute(), _frameCount);


            if (KeyboardState.IsKeyPressed(Keys.Escape))
            {
                CursorState = CursorState == CursorState.Grabbed ? CursorState.Normal : CursorState.Grabbed;
                _sceneManager.SetActiveComponentReferences(new MenuDisableAttribute(), CursorState == CursorState.Grabbed);
                _sceneManager.SetActiveComponentReferences(new MenuEnableAttribute(), CursorState != CursorState.Grabbed);
            }
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
