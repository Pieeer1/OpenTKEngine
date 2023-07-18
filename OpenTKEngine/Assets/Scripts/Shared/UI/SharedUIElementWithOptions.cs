using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.UI;

namespace OpenTKEngine.Assets.Scripts.Shared.UI
{
    public class SharedUIElementWithOptions : SharedUIElement
    {
        public bool SentFromOptions = false;

        public SharedUIElementWithOptions(CanvasComponent canvas) : base(canvas)
        {
        }

        public override void EnableDisableUIElements(bool val) { }
    }
}
