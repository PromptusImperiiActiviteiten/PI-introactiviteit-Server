using PI_introactiviteit_Server.Services;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PI_introactiviteit_Server.IndividualClientHandling.ClientStates
{
    abstract class ClientMessageState
    {
        ActiveClient client;
        protected string clientMessageRegexString { get; } = @"^1\d{2}:";
        protected Regex clientMessageRegex { get; private set; }

        public ClientMessageState(ActiveClient client) {
            this.client = client;
            clientMessageRegex = new Regex(clientMessageRegexString);
        }

        public abstract void HandleClientMessage(string message);

        protected void ChangeRegex(string newRegexString) {
            clientMessageRegex = new Regex(newRegexString);
        }
    }
}
