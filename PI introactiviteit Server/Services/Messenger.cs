using PI_introactiviteit_Server.Models;
using System.Text;

namespace PI_introactiviteit_Server.Services
{
    enum MessageType { 
        SERVER_ALL,
        STANDARD_ALL,
        STANDARD_ALL_BUT_ONE,
        STANDARD_ONE
    }

    class Messenger
    {
        public static void DelegateMessage(MessageType messageType, List<ClientModel> clients, string message)
        {
            string prefix;
            string encodedMessage;

            switch (messageType)
            {
                case MessageType.SERVER_ALL:
                    prefix = "204:";
                    break;
                case MessageType.STANDARD_ALL:
                    prefix = "201:";
                    break;
                default:
                    throw new IndexOutOfRangeException("This message type requires different parameters than provided.");
            }

            encodedMessage = string.Concat(prefix, message);
            MessageAll(clients, message);
        }

        public static void DelegateMessage(MessageType messageType, List<ClientModel> clients, string message, ClientModel seperatedClient)
        {
            string prefix;
            string encodedMessage;

            switch (messageType)
            {
                case MessageType.STANDARD_ALL_BUT_ONE:
                    prefix = "203:";
                    encodedMessage = string.Concat(prefix, message);
                    MessageAllButOne(clients,message,seperatedClient);
                    break;
                case MessageType.STANDARD_ONE:
                    prefix = "202:";
                    encodedMessage = string.Concat(prefix, message);
                    MessageOnlyOne(clients, message, seperatedClient);
                    break;
                default:
                    throw new IndexOutOfRangeException("This message type requires different parameters than provided.");
            }
        }

        private static void MessageAll(List<ClientModel> clients, string message)
        {
            byte[] response = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients)
            {
                client.clientStream.Write(response, 0, response.Length);
            }
        }

        private static void MessageAllButOne(List<ClientModel> clients, string message, ClientModel excludedClient)
        {
            byte[] response = Encoding.UTF8.GetBytes(message);

            foreach (var client in clients)
            {
                if (client == excludedClient) continue;
                client.clientStream.Write(response, 0, response.Length);

            }
        }

        private static void MessageOnlyOne(List<ClientModel> clients, string message, ClientModel client)
        {
            byte[] response = Encoding.UTF8.GetBytes(message);
            client.clientStream.Write(response, 0, response.Length);
        }
    }
}
