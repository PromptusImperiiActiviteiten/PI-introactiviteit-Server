using System.Text.RegularExpressions;

namespace PI_introactiviteit_Server.ServersideClientActions.ClientStates
{
    abstract class ClientMessageState
    {
        ActiveClient client;
        protected abstract string clientMessageRegexString { get; }
        Regex regex { get; }

        public ClientMessageState(ActiveClient client) {
            this.client = client;
            regex = new Regex(clientMessageRegexString);
        }

        public abstract void CheckMessage(string message);
    }
}
