using ImGuiNET;
using OpenTK.Mathematics;

namespace OpenTKEngine.Models.UI
{
    public class Button : UIElement
    {
        private readonly  Action _action;
        private readonly string _label;
        private readonly Vector2 _size;

        public Button(Action action, string label, Vector2 size, ImGuiWindowFlags flags, string name, Vector2? location = null) : base(flags, name, location)
        {
            _action = action;
            _label = label;
            _size = size;
        }

        public override void StartRender()
        {
            base.StartRender();

            if (ImGui.Button(_label, new System.Numerics.Vector2(_size.X, _size.Y)))
            {
                _action.Invoke();
            };
        }
    }
}
