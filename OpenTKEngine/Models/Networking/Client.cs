using ENet;
using System.Text;

namespace OpenTKEngine.Models.Networking
{
    public class Client
    {
        private Host _client;
        public Client(ushort port = 1234, string address = "localhost")
        {
            Library.Initialize();

            _client = new Host();
            _client.Create();

            Address clientAddress = new Address();
            clientAddress.SetHost(address);
            clientAddress.Port = port;

            Peer clientPeer = _client.Connect(clientAddress);
            
        }
        public event EventHandler<NetworkArgs>? OnConnected;
        public event EventHandler<NetworkArgs>? OnDisconnected;
        public event EventHandler<NetworkArgs>? OnPacketReceived;
        public void Update()
        {
            if (_client.Service(0, out Event clientNetEvent) > 0) //TODO CHANGE TO ALLOW POLLING FROM EXAMPLE -> https://github.com/nxrighthere/ENet-CSharp
            {
                switch (clientNetEvent.Type)
                {
                    case EventType.Connect:
                        OnConnection(clientNetEvent);
                        break;

                    case EventType.Disconnect:
                        OnDisconnect(clientNetEvent);
                        break;
                    case EventType.Receive:
                        OnPacketReceiving(clientNetEvent);
                        break;
                }
            }

        }
        protected void OnConnection(Event @event)
        {
            if (OnConnected is not null)
            { 
                OnConnected.Invoke(this, new NetworkArgs(@event));
            }
        }        
        protected void OnDisconnect(Event @event)
        {
            if (OnDisconnected is not null)
            {
                OnDisconnected.Invoke(this, new NetworkArgs(@event));
            }
        }        
        protected void OnPacketReceiving(Event @event)
        {
            if (OnConnected is not null)
            { 
                OnConnected.Invoke(this, new NetworkArgs(@event));
            }
        }
    }
}
