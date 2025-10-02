using System.Text.RegularExpressions;

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

        public abstract void CheckMessage(string message);
    }
}
