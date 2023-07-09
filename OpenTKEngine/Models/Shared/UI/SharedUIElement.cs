using OpenTKEngine.Entities.Components;

namespace OpenTKEngine.Models.Shared.UI
{
    public class SharedUIElement
    {
        protected CanvasComponent _canvas;

        public SharedUIElement(CanvasComponent canvas)
        {
            _canvas = canvas;
        }
    }
}
