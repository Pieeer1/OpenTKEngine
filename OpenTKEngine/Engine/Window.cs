using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTKEngine.Attributes;
using OpenTKEngine.Scenes;
using OpenTKEngine.Services;
using BepuPhysics;
using BepuUtilities;
using BepuPhysics.Collidables;
using OpenTKEngine.Models.Physics;
using static OpenTKEngine.Models.Physics.NarrowPhaseCallbacks;
using BepuPhysics.Constraints;

namespace OpenTKEngine.Engine
{
    public class Window : GameWindow
    {
        private bool _firstMove = true;

        private int _frameCount;
        private Vector2 _lastPos;

        private readonly SceneManager _sceneManager;
        private readonly TimeService _timeService;
        private readonly WindowService _windowService;
        private readonly PhysicsService _physicsService;
        private readonly bool _isFullScreenLaunch;
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings, bool isFullScreenLaunch) : base(gameWindowSettings, nativeWindowSettings)
        {
            _sceneManager = SceneManager.Instance;
            _timeService = TimeService.Instance;
            _windowService = WindowService.Instance;
            _windowService.GameWindowReference = this;
            _isFullScreenLaunch = isFullScreenLaunch;
            _physicsService = PhysicsService.Instance;
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            WindowService.Instance.WindowState = _isFullScreenLaunch ? WindowState.Fullscreen : WindowState.Normal;


            var bufferPool = new BepuUtilities.Memory.BufferPool();
            _physicsService.CollidableMaterials = new CollidableProperty<SimpleMaterial>();
            _physicsService.Simulation = Simulation.Create(bufferPool, new NarrowPhaseCallbacks() { CollidableMaterials = _physicsService.CollidableMaterials }, new PoseIntegratorCallbacks(new System.Numerics.Vector3(0.0f, -10.0f, 0.0f)), new SolveDescription(8, 1));
            var targetThreadCount = int.Max(1, Environment.ProcessorCount > 4 ? Environment.ProcessorCount - 2 : Environment.ProcessorCount - 1);
            _physicsService.ThreadDispatcher = new ThreadDispatcher(targetThreadCount);


            _sceneManager.AddScene(new BaseDebugScene("base debug 1")); // default scene
            _sceneManager.SwapScene(0);
            _sceneManager.LoadScene(0);

            _windowService.ActiveCursorState = CursorState.Grabbed;
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
            
            _timeService.DeltaTime = e.Time;
            _physicsService.Simulation.Timestep((float)e.Time, _physicsService.ThreadDispatcher);
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
