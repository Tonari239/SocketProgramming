using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Program
    {
        public static List<ServerClient> clients;
        public static string hostName = ConfigurationManager.AppSettings.Get("hostName");
        public static int port = int.Parse(ConfigurationManager.AppSettings.Get("portNumber"));

        public static int Main(String[] args)
        {
           
            InitializeClients();
            
            SendDataFromClients();

            CleanUp();
            return 0;
        }

        public static void InitializeClients()
        {
            Console.WriteLine("Enter how many clients you'd like to have.");
            int clientsCount;
            clientsCount = int.Parse(Console.ReadLine());
            clients = new List<ServerClient>();
            for (int i = 0; i < clientsCount; i++)
            {
                clients.Add(new ServerClient(hostName, port));
                clients[i].ConnectToServer();
            }
        }

        public static void SendDataFromClients()
        {
            clients.ForEach(x=>x.SendData("Holy macaroni<EOF>"));
        }

        public static void CleanUp()
        {
            clients.ForEach(x => x.CloseServerConnection());
            Console.WriteLine("Press any key to close.");
            Console.ReadKey();
        }
    }
}