using PI_introactiviteit_Server.ServersideClientActions.ClientStates;

namespace PI_introactiviteit_Server.ServersideClientActions
{
    internal class ActiveClient
    {
        ClientMessageState currentState;

        public ActiveClient() {
            currentState = new Initialising_ClientMessageState(this);
        }



        public void SetState(ClientMessageState newState) {
            currentState = newState;
        }
    }
}
