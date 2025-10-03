using PI_introactiviteit_Server.Models;
using PI_introactiviteit_Server.Services;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PI_introactiviteit_Server.IndividualClientHandling.ClientStates
{
    internal class Initialising_ClientMessageState(ActiveClient client) : ClientMessageState(client)
    {
        protected override string clientMessageRegexString => @"^101:";

        public override void HandleClientMessage(string message)
        {
            if (!MessageAlterations.HandleRegexCheck(message, clientMessageRegex))
            {
                string errorResponseMessage = "This message isn't correctly formatted";
                Messenger.DelegateMessage(MessageType.SERVER_ERROR_ONE, client.activeClient, errorResponseMessage);
                return;
            }


            string clientName;

            if ((clientName = MessageAlterations.IsolateMessageFromProtocol(message)) == null) {
                string errorResponseMessage = "There was an error trying to format the name";
                Messenger.DelegateMessage(MessageType.SERVER_ERROR_ONE, client.activeClient, errorResponseMessage);
                return;
            }

            client.activeClient.setName(clientName);
            FormatAndSendResponse();

            client.SetState(new Chatting_ClientMessageState(client));
        }

        private void FormatAndSendResponse()
        {
            string responseMessage = string.Format("welcome {0}!", client.activeClient.clientName);
            Messenger.DelegateMessage(MessageType.SERVER_ONE, client.activeClient, responseMessage);
        }

    }
}
