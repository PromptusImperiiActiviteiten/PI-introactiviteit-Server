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
            if (HandleMessageFormatCheck(message)) return;

            IsolateAndSetClientName(message);
            FormatAndSendResponse();

            client.SetState(new Chatting_ClientMessageState(client));
        }

        private void IsolateAndSetClientName(string message) {
            string clientName = Regex.Replace(message, clientMessageRegexString, "");
            client.activeClient.setName(clientName);
        }

        private void FormatAndSendResponse()
        {
            string responseMessage = string.Format("welcome {0}!", client.activeClient.clientName);
            Messenger.DelegateMessage(MessageType.SERVER_ONE, client.activeClient, responseMessage);
        }

    }
}
