using Common.Logging;
using System;
using System.Net;
using System.Net.Sockets;
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

        public void SendData(dynamic data)
        {
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(data);
                _serverConnection.Send(msg);
                _loggingService.LogTask(new NetworkingTask(DateTime.Now, Action.SENT_DATA), $"Sent data `{data}` to server, total bytes: {msg.Length}");

                int bytesRec = _serverConnection.Receive(buffer);
                _loggingService.LogTask(new NetworkingTask(DateTime.Now, Action.RECEIVED_DATA), $"Received data `{data}` from server, total bytes: {bytesRec}");
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
