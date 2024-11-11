using Common.DataTransfer;
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
        private const int BUFFER_SIZE = 1024;

        public Server(string hostName, int port, ILoggingService loggingService)
        {
            _loggingService = loggingService;

            IPHostEntry host = Dns.GetHostEntry(hostName);
            ipAddress = host.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddress, port);
        }

        public void StartServer<T>() where T : struct
        {
            try
            {
                Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(localEndPoint);
                listener.Listen(MAX_CLIENTS_IN_QUEUE);

                _loggingService.LogTask(new NetworkingTask(DateTime.Now, Common.Logging.Action.SERVER_STARTED));

                while (true) 
                {
                    Socket handler = listener.Accept();
                    //the presumption is that the client sends the number of items in the array
                    T[] data = ReceiveData<T>(handler);

                    Array.Sort(data);
                    SendData(handler, data);
                   
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                
            }
            catch (Exception e)
            {
                _loggingService.LogError(e);
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
        }

        private T[] ReceiveData<T>(Socket client) where T : struct
        {
            byte[] bytes = new byte[BUFFER_SIZE];
            client.Receive(bytes);

            //the presumption is that the client sends the number of elements first
            int elementsCount = ByteSerializer<int>.Deserialize(bytes);

            //and then the elements of the array
            client.Receive(bytes);
            T[] result = ArrayByteSerializer<T>.Deserialize(bytes);

            _loggingService.LogTask(new NetworkingTask(DateTime.Now, Common.Logging.Action.RECEIVED_DATA), $"Received data `{string.Join(',', result)}` from client");
            return result;
        }

        private void SendData<T>(Socket client, T[] data) where T : struct
        {
            client.Send(ArrayByteSerializer<T>.Serialize(data));
            _loggingService.LogTask(new NetworkingTask(DateTime.Now, Common.Logging.Action.SENT_DATA), $"Sent data {data} to client");
        }
    }
}