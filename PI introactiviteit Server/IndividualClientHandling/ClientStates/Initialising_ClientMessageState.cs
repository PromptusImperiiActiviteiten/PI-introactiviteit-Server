using PI_introactiviteit_Server.Models;
using PI_introactiviteit_Server.Services;
using System.Text;

namespace PI_introactiviteit_Server.IndividualClientHandling.ClientStates
{
    internal class Initialising_ClientMessageState(ActiveClient client) : ClientMessageState(client)
    {
        protected override string clientMessageRegexString => @"101:.*";

        public override void CheckMessage(string message) {
            string responseMessage;

            if (!clientMessageRegex.IsMatch(message)) {
                responseMessage = "This message isn't correctly formatted";
                Messenger.DelegateMessage(MessageType.SERVER_CHAT_ONE, client.activeClient, responseMessage);
                return;
            }

            responseMessage = "Welcome";
            Messenger.DelegateMessage(MessageType.SERVER_ONE, client.activeClient, responseMessage);
        }
    }
}
