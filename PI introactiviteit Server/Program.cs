using PI_introactiviteit_Server.Services;

namespace PI_introactiviteit_Server
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            int CommincaitonsPort = 5000;

            ServerInitialisations server = new ServerInitialisations();
            server.Start(CommincaitonsPort);
        }
    }
}