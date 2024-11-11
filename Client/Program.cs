using Common.Logging;
using System;
using System.Configuration;

namespace Client
{
    class Program
    {
        public static ServerClient[] clients;
        public static string hostName = ConfigurationManager.AppSettings.Get("hostName");
        public static int port = int.Parse(ConfigurationManager.AppSettings.Get("portNumber"));
        public static string logFilePattern = ConfigurationManager.AppSettings.Get("logFile");
        public static int Main(String[] args)
        {
            InitializeClients();
            
            SendDataFromClients();

            CleanUp();
            return 0;
        }

        public static void InitializeClients()
        {
            ILoggingService loggingService;
            string logFileName;
            
            Console.WriteLine("Enter how many clients you'd like to have.");
            int clientsCount;
            clientsCount = int.Parse(Console.ReadLine());

            clients = new ServerClient[clientsCount];

            for (int i = 0; i < clientsCount; i++)
            {
               // logFileName = string.Format(logFilePattern, i);
                loggingService = new ConsoleLoggingService();
                clients[i] = new ServerClient(hostName, port, loggingService);
                clients[i].ConnectToServer();
            }
        }

        public static void SendDataFromClients()
        {
            foreach (ServerClient client in clients)
            {
                client.SendData("Holy macaroni<EOF>");
            }
        }

        public static void CleanUp()
        {
            foreach (ServerClient client in clients)
            {
                client.CloseServerConnection();
            }

            Console.WriteLine("Press any key to close.");
            Console.ReadKey();
        }
    }
}