using PI_introactiviteit_Server.Models;
using PI_introactiviteit_Server.Services;

namespace PI_introactiviteit_Server.IndividualClientHandling.ClientStates
{
    class Chatting_ClientMessageState(ActiveClient client) : ClientMessageState(client)
    {
        public override void HandleClientMessage(string incommingClientMessage) {
            string errorMessage;
            MessageProtocol incommingMessageProtocol;


            if (!MessageAlterations.HandleRegexCheck(incommingClientMessage, clientMessageRegex)) {
                string errorResponseMessage = "This message isn't correctly formatted";
                Messenger.DelegateMessage(MessageProtocol.SERVER_ERROR_ONE, client.activeClient, errorResponseMessage);
                return;
            }

            incommingMessageProtocol = MessageAlterations.GetProtocolFromMessage(incommingClientMessage);

            switch (incommingMessageProtocol) {
                case MessageProtocol.CLIENT_CHAT_ALL:
                    FormatAndSendResponse(incommingClientMessage);
                    break;
                case MessageProtocol.CLIENT_CHAT_WHISPER:
                    ClientModel isolatedClient;
                    string isolatedClientName;

                    if ((isolatedClientName = MessageAlterations.GetTargetedClientNameFrom102Message(incommingClientMessage)) == null) {
                        errorMessage = "this message does not conform to the whisper command.";
                        FormatAndSendResponse(MessageProtocol.SERVER_ERROR_ONE, client.activeClient, errorMessage);
                        break;
                    }

                    if ((isolatedClient = ServerInitialisations.IsolateClientModelByName(client.server.clients, isolatedClientName)) == null) {
                        errorMessage = "this person does not exist";
                        FormatAndSendResponse(MessageProtocol.SERVER_ERROR_ONE, client.activeClient, errorMessage);
                        break;
                    }

                    FormatAndSendResponse(MessageProtocol.SERVER_CHAT_ONE, isolatedClient, incommingClientMessage);
                    break;
                case MessageProtocol.CLIENT_LOGIN_MESSAGE:
                    errorMessage = "This message uses the login protocol and is not meant for chat usage.";
                    FormatAndSendResponse(MessageProtocol.SERVER_ERROR_ONE, client.activeClient, errorMessage);
                    break;
                default:
                    errorMessage = "this message is not a recognised command.";
                    Messenger.DelegateMessage(MessageProtocol.SERVER_ERROR_ONE, client.activeClient, errorMessage);
                    break;
            }
        }

        private void FormatAndSendResponse(string unformattedOutgoingMessage) {
            string messageWithoutProtocol = MessageAlterations.RemoveProtocolFromMessage(unformattedOutgoingMessage);
            string responseMessage = string.Format("{0}: {1}", client.activeClient.clientName, messageWithoutProtocol);

            Messenger.DelegateMessage(MessageProtocol.SERVER_CHAT_ALL_BUT_ONE, client.server.clients, responseMessage, client.activeClient);
        }

        private void FormatAndSendResponse(MessageProtocol messageType, ClientModel isolatedClient, string unformattedOutgoingMessage) {
            string messageWithoutProtocol = MessageAlterations.RemoveClientNameAndProtocolFrom102Message(unformattedOutgoingMessage);
            string responseMessage = string.Format("{0}: {1}", client.activeClient.clientName, messageWithoutProtocol);
            Messenger.DelegateMessage(messageType, isolatedClient, responseMessage);
        }
    }
}
