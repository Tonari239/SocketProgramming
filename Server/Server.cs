using Common.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    public class Server
    {
        private readonly ILoggingService _loggingService;
        private readonly IPAddress ipAddress;
        private readonly int MAX_CLIENTS_IN_QUEUE = 10;
        private readonly IPEndPoint localEndPoint;
        private const string EOF_INDICATOR = "<EOF>";
        private const string ACKNOWLEDGE_INDICATOR = "ACK";
        private const int BUFFER_SIZE = 1024;

        public Server(string hostName, int port, ILoggingService loggingService)
        {
            _loggingService = loggingService;

            IPHostEntry host = Dns.GetHostEntry(hostName);
            ipAddress = host.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddress, port);
        }

        public void StartServer()
        {
            try
            {
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(MAX_CLIENTS_IN_QUEUE);

                _loggingService.LogTask(new NetworkingTask(DateTime.Now, Common.Logging.Action.SERVER_STARTED));
                Socket handler = listener.Accept();

                string data = ReceiveData(handler);;

                SendData(handler, ACKNOWLEDGE_INDICATOR);
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception e)
            {
                _loggingService.LogError(e);
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
        }

        private string ReceiveData(Socket client)
        {
            string data = null;
            byte[] bytes = null;

            while (true)
            {
                bytes = new byte[BUFFER_SIZE];
                int bytesRec = client.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                if (data.IndexOf(EOF_INDICATOR) > -1)
                {
                    break;
                }
            }

            _loggingService.LogTask(new NetworkingTask(DateTime.Now, Common.Logging.Action.RECEIVED_DATA));
            return data;
        }

        private void SendData(Socket client, string data)
        {
            client.Send(Encoding.ASCII.GetBytes(data));
            _loggingService.LogTask(new NetworkingTask(DateTime.Now, Common.Logging.Action.SEND_DATA));
        }
    }
}