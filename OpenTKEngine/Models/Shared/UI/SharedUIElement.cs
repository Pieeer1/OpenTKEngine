using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.UI;
using System.Reflection;

namespace OpenTKEngine.Models.Shared.UI
{
    public abstract class SharedUIElement
    {
        protected CanvasComponent _canvas;

        public SharedUIElement(CanvasComponent canvas)
        {
            _canvas = canvas;
        }
        public abstract void EnableDisableUIElements(bool val);
    }
}
