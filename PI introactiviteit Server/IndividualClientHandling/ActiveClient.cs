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
                while ((bytesRead = activeClient.clientStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(message);
                    currentState.HandleClientMessage(message);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine("Error communicating with client: " + ex.Message);
                string disconnectMessage = string.Format("{0} Has disconnected from the server", activeClient.clientName);
                Messenger.DelegateMessage(MessageProtocol.SERVER_ALL_BUT_ONE, server.clients, disconnectMessage,activeClient);
            }
            catch (Exception ex)
            {
                string disconnectMessage = string.Format("{0} Has disconnected from the server", activeClient.clientName);

                Console.WriteLine("an unknown error has occured causing the client: {0} to disconnect", activeClient.clientName);
                Messenger.DelegateMessage(MessageProtocol.SERVER_ERROR_ONE, activeClient, 
                    "Het spijt ons, er gaat iets fout bij de server." +
                    "Dit komt niet door jou, wij hebben iets over het hoofd gezien." +
                    "Maar, je moet wel het programma even opnieuw opstarten.");
                Messenger.DelegateMessage(MessageProtocol.SERVER_ALL, server.clients, disconnectMessage);
            }
            finally
            {   
                activeClient.clientStream.Close();
                activeClient.tcpClient.Close();
                server.clients.Remove(activeClient);
                Console.WriteLine("Client disconnected.");
            }
        }

        public void SetState(ClientMessageState newState) {
            currentState = newState;
        }
    }
}
