using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTKEngine.Services;
using static OpenTKEngine.Models.Constants;

namespace OpenTKEngine.Models.UI
{
    public class Button : UIElement
    {
        private readonly  Action _action;
        private readonly string _label;
        private readonly Vector2 _size;

        public Button(Action action, string label, Vector2 size, ImGuiWindowFlags flags, string name, Vector2? location = null, Keys toggleKey = Keys.Unknown) : base(flags, name, location, toggleKey)
        {
            _action = action;
            _label = label;
            _size = size;
        }

        public override void StartRender()
        {
            base.StartRender();

            ImGuiStylePtr style = ImGui.GetStyle();

            style.Colors[(int)ImGuiCol.Button] = DataManipulationService.OpenTKVectorToSystemVector(Styles.ColorDictionary["ButtonColor"]);
            ImGui.PushStyleColor(ImGuiCol.Text, DataManipulationService.OpenTKVectorToSystemVector(Styles.ColorDictionary["DefaultText"]));
            if (ImGui.Button(_label, new System.Numerics.Vector2(_size.X, _size.Y)))
            {
                _action.Invoke();
            };
            ImGui.PopStyleColor();
        }
    }
}
