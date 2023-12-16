using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class ServerClient
    {
        private readonly Socket _serverConnection;
        private readonly IPEndPoint _remoteEndPoint;
        private byte[] buffer = new byte[1024];

        private static int clientId = 0;
        public ServerClient(string serverName, int port)
        {
            IPAddress _serverIpAddress = Dns.GetHostEntry(serverName).AddressList[0];
            _remoteEndPoint = new IPEndPoint(_serverIpAddress, port);
            _serverConnection = new Socket(_serverIpAddress.AddressFamily,
                   SocketType.Stream, ProtocolType.Tcp);
            clientId++;
        }

        public void ConnectToServer()
        {
            _serverConnection.Connect(_remoteEndPoint);

            Console.WriteLine($"Socket with id {clientId} connected to server.");
        }

        public void SendData(dynamic data)
        {
            try
            {
                byte[] msg = Encoding.ASCII.GetBytes(data);

                _serverConnection.Send(msg);

                int bytesRec = _serverConnection.Receive(buffer);
                Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(buffer, 0, bytesRec));
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        public void CloseServerConnection()
        {
            _serverConnection.Shutdown(SocketShutdown.Both);
            _serverConnection.Close();
        }
    }
}
