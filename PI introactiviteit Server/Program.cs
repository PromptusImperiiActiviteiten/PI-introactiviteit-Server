namespace PI_introactiviteit_Server
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            SimpleServer server = new SimpleServer();
            server.Start(5000);
            //ApplicationConfiguration.Initialize();
            //Application.Run(new Form1());
        }
    }
}