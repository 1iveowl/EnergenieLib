using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using EnergenieLibv2.Extensions;

namespace EnergenieLibv2.Helper
{
    internal static class TcpSocketHelper
    {
        internal static async Task<Socket> GetTcpSocket(string host, int port, int timeout)
        {
            SocketPermission permission = new SocketPermission(
                NetworkAccess.Connect,
                TransportType.Tcp,
                "",
                port);

            var ipEndPoint = GetIpEndpoint(host, port);

            var tcpSocket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = false
            };
            await tcpSocket.ConnectAsync(ipEndPoint, timeout);
            return tcpSocket;
        }

        internal static byte[] GetResponse(Socket tcpSocket)
        {
            byte[] responseBuffer = new byte[1024];
            int numberOfBytesReceived = tcpSocket.Receive(responseBuffer);

            while (tcpSocket.Available > 0)
            {
                numberOfBytesReceived = tcpSocket.Receive(responseBuffer);
            }

            byte[] response = new byte[numberOfBytesReceived];
            for (int i = 0; i < numberOfBytesReceived; i++)
            {
                response[i] = responseBuffer[i];
            }
            return response;
        }

        private static IPEndPoint GetIpEndpoint(string host, int port)
        {
            IPAddress ipAddress;

            //Handles both ipadresses and dns names. 
            if (!IPAddress.TryParse(host, out ipAddress))
            {
                try
                {
                    var ipHostAddress = Dns.GetHostEntry(host);
                    ipAddress = ipHostAddress.AddressList.FirstOrDefault();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
            return new IPEndPoint(ipAddress, port);
        }
    }
}
