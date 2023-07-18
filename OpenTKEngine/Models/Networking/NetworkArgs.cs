using ENet;

namespace OpenTKEngine.Models.Networking
{
    public class NetworkArgs : EventArgs
    {
        public Event NetworkEvent { get; set; }

        public NetworkArgs(Event networkEvent)
        {
            NetworkEvent = networkEvent;
        }
    }
}
