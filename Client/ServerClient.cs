using Common.DataTransfer;
using Common.Logging;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using Action = Common.Logging.Action;

namespace Client
{
    class ServerClient
    {
        private readonly ILoggingService _loggingService;
        private readonly Socket _serverConnection;
        private readonly IPEndPoint _remoteEndPoint;
        private byte[] buffer = new byte[1024];

        private static int clientId = 0;
        public ServerClient(string serverName, int port, ILoggingService loggingService)
        {
            _loggingService = loggingService;
            IPAddress _serverIpAddress = Dns.GetHostEntry(serverName).AddressList[0];
            _remoteEndPoint = new IPEndPoint(_serverIpAddress, port);
            _serverConnection = new Socket(_serverIpAddress.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
            clientId++;
        }

        public void ConnectToServer()
        {
            _serverConnection.Connect(_remoteEndPoint);

            _loggingService.LogTask(new NetworkingTask(DateTime.Now, Action.CLIENT_CONNECTED),
                $"Socket with id {clientId} connected to server.");
        }

        public void SendData<T>(T[] data) where T : struct
        {
            try
            {
                //The presumption is that we are going to send an array. We first send out the number of elements in the array:
                _serverConnection.Send(ByteSerializer<int>.Serialize(data.Length));
                _loggingService.LogTask(new NetworkingTask(DateTime.Now, Action.SENT_DATA), $"Sent data `{data.Length}` to server");

                //Now we send the actual array
                _serverConnection.Send(ArrayByteSerializer<T>.Serialize(data));
                _loggingService.LogTask(new NetworkingTask(DateTime.Now, Action.SENT_DATA), $"Sent data `{String.Join(',',data)}` to server");

                //The presumption is that the server is going to send back the sorted array:
                int bytesRec = _serverConnection.Receive(buffer);
                T[] resultData = ArrayByteSerializer<T>.Deserialize(buffer);

                _loggingService.LogTask(new NetworkingTask(DateTime.Now, Action.RECEIVED_DATA), $"Received data `{resultData}` from server");
            }
            catch (Exception e)
            {
                _loggingService.LogError(e);
            }
        }

        public void CloseServerConnection()
        {
            _serverConnection.Shutdown(SocketShutdown.Both);
            _serverConnection.Close();
            _loggingService.LogTask(new NetworkingTask(DateTime.Now, Action.CLIENT_DISCONNECTED));
        }
    }
}
