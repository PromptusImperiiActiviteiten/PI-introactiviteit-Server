using PI_introactiviteit_Server.Services;

namespace PI_introactiviteit_Server
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ServerInitialisations server = new ServerInitialisations();
            server.Start(5000);
        }
    }
}