namespace PI_introactiviteit_Server.IndividualClientHandling.ClientStates
{
    internal class Initialising_ClientMessageState(ActiveClient client) : ClientMessageState(client)
    {
        protected override string clientMessageRegexString => @"101:.*";

        public override void CheckMessage(string message) {
            throw new NotImplementedException();
        }
    }
}
