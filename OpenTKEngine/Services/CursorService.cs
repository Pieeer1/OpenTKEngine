using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace OpenTKEngine.Services
{
    public class CursorService : SingletonService<CursorService>
    { 
        private CursorService() { }
        public GameWindow GameWindowReference { get; set; } = null!;
        public CursorState ActiveCursorState { get => GameWindowReference.CursorState; set => GameWindowReference.CursorState = value; }
    }
}
