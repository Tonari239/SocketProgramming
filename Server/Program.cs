using Common.Logging;
using System;
using System.Configuration;

namespace Server
{
    class Program
    {

        public static Server server;

        public static int Main(String[] args)
        {
            Initialize();

            server.StartServer<int>();
            return 0;
        }

        public static void Initialize()
        {
            string hostName = ConfigurationManager.AppSettings.Get("hostName");
            int port = int.Parse(ConfigurationManager.AppSettings.Get("portNumber"));

            ILoggingService loggingService = new ConsoleLoggingService();
            server = new Server(hostName, port, loggingService);

        }

    }
}