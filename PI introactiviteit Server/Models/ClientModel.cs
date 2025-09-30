using System.Net.Sockets;

namespace PI_introactiviteit_Server.Models
{
    public class ClientModel
    {
        public string clientName { get; private set; }
        public TcpClient tcpClient { get; }
        public NetworkStream clientStream { get; }

        public ClientModel(string clientName, TcpClient tcpClient, NetworkStream clientStream)
        {
            this.clientName = clientName;
            this.tcpClient = tcpClient;
            this.clientStream = clientStream;
        }

        public void setName(string newName)
        {
            if (clientName != null) return;
            clientName = newName;
        }

    }
}
