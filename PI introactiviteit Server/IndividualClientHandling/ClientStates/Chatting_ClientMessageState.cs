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
            if (!HandleRegexCheck(message)) return;

            string messageCode = Regex.Replace(message,@":.*","");
            string errorMessage;
            switch (messageCode)
            {
                case "102":
                    FormatAndSendResponse(message);
                    break;
                case "103":
                    string messageEndRegexString = @";.*$";
                    ChangeRegex(messageEndRegexString);
                    if (!HandleRegexCheck(message)) return;

                    string whisperClientName = Regex.Replace(message,clientMessageRegexString,"");
                    whisperClientName = Regex.Replace(whisperClientName,messageEndRegexString,"");

                    foreach (ClientModel client in client.server.clients)
                    {
                        if (!client.clientName.Equals(whisperClientName)) continue;
                        FormatAndSendResponse(MessageType.SERVER_CHAT_ONE,client,message);
                        ChangeRegex(clientMessageRegexString);
                        return;
                    }
                    errorMessage = "this person does not exist";
                    FormatAndSendResponse(MessageType.SERVER_ERROR_ONE, client.activeClient, errorMessage);
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

        private void FormatAndSendResponse(MessageType messageType,ClientModel client, string message)
        {
            Messenger.DelegateMessage(messageType, client, message);
        }
    }
}
