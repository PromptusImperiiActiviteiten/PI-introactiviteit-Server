using PI_introactiviteit_Server.IndividualClientHandling.ClientStates;
using PI_introactiviteit_Server.Models;
using PI_introactiviteit_Server.Services;
using System.Net.Sockets;
using System.Text;


namespace PI_introactiviteit_Server.IndividualClientHandling
{
    internal class ActiveClient
    {

        public ServerInitialisations server {get;}
        ClientMessageState currentState;
        public ClientModel activeClient { get; private set; }


        public ActiveClient(ServerInitialisations server)
        {
            currentState = new Initialising_ClientMessageState(this);
            this.server = server;
        }

        public void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int bytesRead;
            
            activeClient = new ClientModel(client, stream);

            server.clients.Add(activeClient);
            try
            {
                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: " + message);
                    currentState.CheckMessage(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error communicating with client: " + ex.Message);
            }
            finally
            {
                stream.Close();
                client.Close();
                server.clients.Remove(activeClient);
                Console.WriteLine("Client disconnected.");
            }
        }

        public void SetState(ClientMessageState newState) {
            currentState = newState;
        }
    }
}
