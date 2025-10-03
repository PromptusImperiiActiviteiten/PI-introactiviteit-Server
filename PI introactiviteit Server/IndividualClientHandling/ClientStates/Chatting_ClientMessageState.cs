using PI_introactiviteit_Server.Models;
using PI_introactiviteit_Server.Services;
using System.Text.RegularExpressions;

namespace PI_introactiviteit_Server.IndividualClientHandling.ClientStates
{
    class Chatting_ClientMessageState(ActiveClient client) : ClientMessageState(client)
    {
        protected override string clientMessageRegexString => @"^1(?!01)\d{2}:";

        public override void HandleClientMessage(string message)
        {
            if (!MessageAlterations.HandleRegexCheck(message,clientMessageRegex)) {
                string errorResponseMessage = "This message isn't correctly formatted";
                Messenger.DelegateMessage(MessageType.SERVER_ERROR_ONE, client.activeClient, errorResponseMessage);
                return;
            } 

            string messageCode = MessageAlterations.IsolateProtocolFromMessage(message);
            string errorMessage;
            switch (messageCode)
            {
                case "102":
                    FormatAndSendResponse(message);
                    break;
                case "103":
                    ClientModel isolatedClient;
                    string isolatedClientName;

                    if ((isolatedClientName = MessageAlterations.IsolateClientNameFrom103Message(message)) == null) {
                        errorMessage = "this message does not conform to the whisper command";
                        FormatAndSendResponse(MessageType.SERVER_ERROR_ONE, client.activeClient, errorMessage);
                        break;
                    }

                    if ((isolatedClient = ServerInitialisations.IsolateClientModelByName(client.server.clients, isolatedClientName)) == null)
                    {
                        errorMessage = "this person does not exist";
                        FormatAndSendResponse(MessageType.SERVER_ERROR_ONE, client.activeClient, errorMessage);
                        break;
                    }

                    FormatAndSendResponse(MessageType.SERVER_CHAT_ONE, isolatedClient, message);

                    break;
                default:
                    errorMessage = "this message is not a recognised command";
                    FormatAndSendResponse(MessageType.SERVER_ERROR_ONE,client.activeClient, errorMessage);
                    break;
            }
        }

        private void FormatAndSendResponse(string message)
        {
            string messageWithoutProtocol = Regex.Replace(message, clientMessageRegexString, "");
            string responseMessage = string.Format("{0}: {1}", client.activeClient.clientName, messageWithoutProtocol);

            Messenger.DelegateMessage(MessageType.SERVER_CHAT_ALL_BUT_ONE, client.server.clients, responseMessage, client.activeClient);
        }

        private void FormatAndSendResponse(MessageType messageType,ClientModel isolatedClient, string message)
        {
            string messageWithoutProtocol = Regex.Replace(message, @".*;", "");
            string responseMessage = string.Format("{0}: {1}", client.activeClient.clientName, messageWithoutProtocol);
            Messenger.DelegateMessage(messageType, isolatedClient, responseMessage);
        }
    }
}
