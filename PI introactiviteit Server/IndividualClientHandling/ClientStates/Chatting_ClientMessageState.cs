using PI_introactiviteit_Server.Services;
using System.Text.RegularExpressions;

namespace PI_introactiviteit_Server.IndividualClientHandling.ClientStates
{
    class Chatting_ClientMessageState(ActiveClient client) : ClientMessageState(client)
    {
        protected override string clientMessageRegexString => @"^1(?!01)\d{2}:";

        public override void HandleClientMessage(string message) {
            if (HandleMessageFormatCheck(message)) return;
            FormatAndSendResponse(message);
        }

        private void FormatAndSendResponse(string message)
        {
            string messageWithoutProtocol = Regex.Replace(message, clientMessageRegexString, "");
            string responseMessage = string.Format("{0}: {1}", client.activeClient.clientName, messageWithoutProtocol);
            
            Messenger.DelegateMessage(MessageType.SERVER_CHAT_ALL_BUT_ONE, client.server.clients, responseMessage,client.activeClient);
        }
    }
}
