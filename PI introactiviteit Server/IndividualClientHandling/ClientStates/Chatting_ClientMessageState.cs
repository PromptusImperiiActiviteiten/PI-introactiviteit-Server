namespace PI_introactiviteit_Server.IndividualClientHandling.ClientStates
{
    class Chatting_ClientMessageState(ActiveClient client) : ClientMessageState(client)
    {
        protected override string clientMessageRegexString => @"1(?!01)\d{2}:.*";

        public override void CheckMessage(string message) {
            throw new NotImplementedException();
        }
    }
}
