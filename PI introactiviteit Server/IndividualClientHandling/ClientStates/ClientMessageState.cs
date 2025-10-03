using PI_introactiviteit_Server.Services;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PI_introactiviteit_Server.IndividualClientHandling.ClientStates
{
    abstract class ClientMessageState
    {
        ActiveClient client;
        protected abstract string clientMessageRegexString { get; }
        protected Regex clientMessageRegex { get; private set; }

        public ClientMessageState(ActiveClient client) {
            this.client = client;
            clientMessageRegex = new Regex(clientMessageRegexString);
        }

        public abstract void HandleClientMessage(string message);

        protected Boolean HandleRegexCheck(string message) {
            if (clientMessageRegex.IsMatch(message))
            {
                return true;
            }

            string errorResponseMessage = "This message isn't correctly formatted";
            Messenger.DelegateMessage(MessageType.SERVER_ERROR_ONE, client.activeClient, errorResponseMessage);
            return false;
        }

        protected void ChangeRegex(string newRegexString) {
            clientMessageRegex = new Regex(newRegexString);
        }
    }
}
