using PI_introactiviteit_Server.IndividualClientHandling;
using PI_introactiviteit_Server.Models;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace PI_introactiviteit_Server.Services
{
    public class ServerInitialisations
    {
        private TcpListener listener;
        public List<ClientModel> clients { get; } = new List<ClientModel>();

        public void Start(int port) {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine("Server started, waiting for connections...");

            Thread acceptThread = new Thread(AcceptClients);
            acceptThread.Start();
        }

        private void AcceptClients() {
            while (true) {
                try {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Client connected.");
                    ActiveClient newClient = new ActiveClient(this);

                    Thread clientThread = new Thread(() => newClient.HandleClient(client));
                    clientThread.Start();
                } catch (Exception ex) {
                    Console.WriteLine("Error accepting client: " + ex.Message);
                }
            }
        }


        public static ClientModel IsolateClientModelByName(List<ClientModel> allClients,string clientName) {
            foreach (ClientModel client in allClients)
            {
                if (!client.clientName.Equals(clientName)) continue;
                return client;
            }

            return null;
        }

    }
}
