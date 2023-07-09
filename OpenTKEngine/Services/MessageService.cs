using OpenTK.Mathematics;
using System.Diagnostics;

namespace OpenTKEngine.Services
{
    public class MessageService : SingletonService<MessageService>
    {
        private MessageService() { }

        public event EventHandler<MessageEventArgs>? OnNewMessage;
        public void LogChat(string msg, double ttl = 10.0d)
        {
            Message(msg, new Vector4(1.0f, 1.0f, 1.0f, 1.0f), ttl);
        }
        public void LogInformation(string msg, double ttl = 10.0d)
        {
            Message(msg, new Vector4(1.0f, 1.0f, 1.0f, 1.0f), ttl);
            Debug.WriteLine(msg, "Information");
        }
        public void LogWarning(string msg, double ttl = 10.0d)
        {
            Message($"Warning: {msg}", new Vector4(1.0f, 0.92f, 0.0f, 1.0f), ttl);
            Debug.WriteLine(msg, "Warning");
        }
        public void LogError(string msg, double ttl = 10.0d)
        {
            Message($"Error : {msg}", new Vector4(0.86f, 0.08f, 0.24f, 1.0f), ttl);
            Debug.WriteLine(msg, "Error");
        }
        private void Message(string msg, Vector4 color, double ttl)
        {
            if (OnNewMessage != null)
            {
                OnNewMessage.Invoke(this, new MessageEventArgs(msg, color, ttl));
            }
        }
    }
    public class MessageEventArgs : EventArgs
    {
        public MessageEventArgs(string message, Vector4 color, double timeToLive)
        {
            Message = message;
            Color = color;
            TimeToLive = timeToLive;
        }

        public string Message { get; set; }
        public Vector4 Color { get; set; }
        public double TimeToLive { get; set; }
    }
}
