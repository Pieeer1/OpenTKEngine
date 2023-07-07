using ImGuiNET;
using OpenTK.Mathematics;

namespace OpenTKEngine.Models.UI
{
    public class Button : UIElement
    {
        private Action _action;
        private string _label;
        private Vector2 _position;

        public Button(Action action, string label, Vector2 position, ImGuiWindowFlags flags, string name) : base(flags, name)
        {
            _action = action;
            _label = label;
            _position = position;
        }

        public override void StartRender()
        {
            base.StartRender();

            if (ImGui.Button(_label, new System.Numerics.Vector2(_position.X, _position.Y)))
            {
                _action.Invoke();
            };
        }
    }
}
