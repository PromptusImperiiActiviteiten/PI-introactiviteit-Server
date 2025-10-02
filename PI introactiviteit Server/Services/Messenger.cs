using PI_introactiviteit_Server.Models;
using System.Runtime.Serialization;
using System.Text;

namespace PI_introactiviteit_Server.Services
{
    enum MessageType {
        //client messages from server
        SERVER_CHAT_ALL             = 201,
        SERVER_CHAT_ONE             = 202,
        SERVER_CHAT_ALL_BUT_ONE     = 203,

        //server message
        SERVER_ALL                  = 211,
        SERVER_ONE                  = 212,
        SERVER_ALL_BUT_ONE          = 213,

        //server error messages
        SERVER_ERROR_ALL            = 221,
        SERVER_ERROR_ONE            = 222
        
    }

    class Messenger
    {
        public static void DelegateMessage(MessageType messageType, List<ClientModel> clients, string message)
        {
            string prefix;
            string encodedMessage;

            if (!(
                messageType == MessageType.SERVER_CHAT_ALL      ||
                messageType == MessageType.SERVER_ALL           ||
                messageType == MessageType.SERVER_ERROR_ALL     ))
            {
                throw new IndexOutOfRangeException("This message type requires different parameters than provided.");
            }

            prefix = Enum.Format(typeof(MessageType), messageType, "d") + ":";
            encodedMessage = string.Concat(prefix, message);
            MessageAll(clients, encodedMessage);
        }

        public static void DelegateMessage(MessageType messageType, ClientModel seperatedClient, string message)
        {
            string prefix;
            string encodedMessage;

            if (!(
                messageType == MessageType.SERVER_CHAT_ONE  ||
                messageType == MessageType.SERVER_ONE       ||
                messageType == MessageType.SERVER_ERROR_ONE ))
            {
                throw new IndexOutOfRangeException("This message type requires different parameters than provided.");
            }

            prefix = Enum.Format(typeof(MessageType), messageType, "d") + ":";
            encodedMessage = string.Concat(prefix, message);
            MessageOnlyOne(encodedMessage, seperatedClient);
        }

        public static void DelegateMessage(MessageType messageType, List<ClientModel> clients, string message, ClientModel seperatedClient)
        {
            string prefix;
            string encodedMessage;

            if (!(
                messageType == MessageType.SERVER_CHAT_ALL_BUT_ONE  ||
                messageType == MessageType.SERVER_ALL_BUT_ONE       ))
            {
                throw new IndexOutOfRangeException("This message type requires different parameters than provided.");
            }

            prefix = Enum.Format(typeof(MessageType), messageType, "d") + ":";
            encodedMessage = string.Concat(prefix, message);
            MessageAllButOne(clients, encodedMessage, seperatedClient);
        }

        private static void MessageAll(List<ClientModel> clients, string message)
        {
            byte[] response = Encoding.UTF8.GetBytes(message);
            foreach (var client in clients)
            {
                client.clientStream.Write(response, 0, response.Length);
                
            }
            Console.WriteLine(message);
        }

        private static void MessageAllButOne(List<ClientModel> clients, string message, ClientModel excludedClient)
        {
            byte[] response = Encoding.UTF8.GetBytes(message);

            foreach (var client in clients)
            {
                if (client == excludedClient) continue;
                client.clientStream.Write(response, 0, response.Length);
                
            }
            Console.WriteLine(message);
        }

        private static void MessageOnlyOne(string message, ClientModel client)
        {
            byte[] response = Encoding.UTF8.GetBytes(message);
            client.clientStream.Write(response, 0, response.Length);
            Console.WriteLine(message);
        }
    }
}
