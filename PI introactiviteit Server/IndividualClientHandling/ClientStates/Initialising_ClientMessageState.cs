using PI_introactiviteit_Server.Models;
using PI_introactiviteit_Server.Services;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PI_introactiviteit_Server.IndividualClientHandling.ClientStates
{
    internal class Initialising_ClientMessageState(ActiveClient client) : ClientMessageState(client)
    {
        public override void HandleClientMessage(string incommingClientMessage)
        {
            string clientName;
            string errorResponseMessage;
            MessageProtocol incommingMessageProtocol;

            if (!MessageAlterations.HandleRegexCheck(incommingClientMessage, clientMessageRegex))
            {
                errorResponseMessage = "This message isn't correctly formatted";
                Messenger.DelegateMessage(MessageProtocol.SERVER_ERROR_ONE, client.activeClient, errorResponseMessage);
                return;
            }

            incommingMessageProtocol = MessageAlterations.GetProtocolFromMessage(incommingClientMessage);

            switch (incommingMessageProtocol) {
                case MessageProtocol.CLIENT_LOGIN_MESSAGE:
                    if ((clientName = MessageAlterations.RemoveProtocolFromMessage(incommingClientMessage)) == null)
                    {
                        errorResponseMessage = "There was an error trying to format the name, try again";
                        Messenger.DelegateMessage(MessageProtocol.SERVER_ERROR_ONE, client.activeClient, errorResponseMessage);
                        return;
                    }
                    break;
                default:
                    errorResponseMessage = "This message is not a Client Login Message";
                    Messenger.DelegateMessage(MessageProtocol.SERVER_ERROR_ONE, client.activeClient, errorResponseMessage);
                    return;
            }

            client.activeClient.setName(clientName);
            FormatAndSendResponse();

            client.SetState(new Chatting_ClientMessageState(client));
        }

        private void FormatAndSendResponse()
        {
            string responseMessage = string.Format("welcome {0}!", client.activeClient.clientName);
            Messenger.DelegateMessage(MessageProtocol.SERVER_ALL, client.server.clients, responseMessage);        }

    }
}
