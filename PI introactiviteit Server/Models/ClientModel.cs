using System.Net.Sockets;

namespace PI_introactiviteit_Server.NewFolder
{
    internal class ClientModel
    {
        string clientName { get; }
        TcpClient tcpClient { get; }
        NetworkStream clientStream { get; }

        public ClientModel(string clientName, TcpClient tcpClient, NetworkStream clientStream) {
            this.clientName = clientName;
            this.tcpClient = tcpClient;
            this.clientStream = clientStream;
        }
    }
}
