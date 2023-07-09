using ImGuiNET;
using OpenTKEngine.Entities.Components;
using OpenTKEngine.Models.UI;
using OpenTKEngine.Services;

namespace OpenTKEngine.Models.Shared.UI
{
    public class ChatBox : SharedUIElement
    {
        private Dictionary<Label, double> labelLifeTracker = new Dictionary<Label, double>();
        private const ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoScrollbar;
        public ChatBox(CanvasComponent canvas) : base(canvas)
        {
            _canvas = canvas;
            MessageService.Instance.OnNewMessage += Instance_OnNewMessage;
            TimeService.Instance.OnUpdateTriggered += Instance_OnUpdateTriggered;
        }

        private void Instance_OnUpdateTriggered(object? sender, TimeArgs e)
        {
            foreach (KeyValuePair<Label, double> labelLife  in labelLifeTracker)
            {
                labelLifeTracker[labelLife.Key] -= e.DeltaTime;
                if (labelLife.Value < 0)
                {
                    _canvas.RemoveUIElement(labelLife.Key);
                }
            }
        }

        private void Instance_OnNewMessage(object? sender, MessageEventArgs e)
        {
            Label newLabel = new Label(string.Empty, windowFlags, "chat", location: new OpenTK.Mathematics.Vector2(500.0f, 500.0f));
            newLabel.Text = e.Message;
            newLabel.Color = e.Color;
            _canvas.AddUIElement(newLabel);
            newLabel.IsActive = true;


            labelLifeTracker.Add(newLabel, e.TimeToLive);
        }
    }
}
