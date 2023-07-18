using ENet;

namespace OpenTKEngine.Models.Networking
{
    public class Server
    {
        private Host _server;
        public Client? Client { get; private set; }
        public Server(ushort port = 1234, string address = "localhost", bool serverAndClient = true)
        { 
            Library.Initialize();
            _server = new Host();
            Address serverAddress = new Address();
            serverAddress.Port = port;
            serverAddress.SetHost(address);
            _server.Create(serverAddress, 32);

            if (serverAndClient)
            {
                Client = new Client(port, address);
            }
            
        }
        public event EventHandler<NetworkArgs>? OnConnected;
        public event EventHandler<NetworkArgs>? OnDisconnected;
        public event EventHandler<NetworkArgs>? OnPacketReceived;
        public void Update()
        {
            if (_server.Service(0, out Event netEvent) > 0)
            {
                switch (netEvent.Type)
                {
                    case EventType.Connect:
                        OnConnection(netEvent);
                        break;

                    case EventType.Disconnect:
                        OnDisconnect(netEvent);
                        break;

                    case EventType.Receive:
                        OnPacketReceiving(netEvent);
                        break;
                }
            }
            if (Client is not null)
            {
                Client.Update();
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
