using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTKEngine.Services
{
    public class WindowService : SingletonService<WindowService>
    { 
        private WindowService() { }
        public GameWindow GameWindowReference { get; set; } = null!;
        public CursorState ActiveCursorState { get => GameWindowReference.CursorState; set => GameWindowReference.CursorState = value; }
        public WindowState WindowState { get => GameWindowReference.WindowState; set => GameWindowReference.WindowState = value; }
    }
}
