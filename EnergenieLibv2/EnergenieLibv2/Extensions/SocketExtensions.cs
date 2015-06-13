using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using static EnergenieLibv2.Helper.RunAsyncHelper;
using static EnergenieLibv2.Helper.TcpSocketHelper;

namespace EnergenieLibv2.Extensions
{
    public static class SocketExtensions
    {
        public static async Task<bool> ConnectAsync(this Socket tcpSocket, EndPoint ipEndPoint, int timeout)
        {
            return await RunAsyncWithTimeOut<bool>(delegate
            {
                tcpSocket.Connect(ipEndPoint);
                return tcpSocket.Connected;
            }, timeout);
        }

        public static async Task<byte[]> SendReceiveAsync(this Socket tcpSocket, byte[] message, int timeout)
        {

            return await RunAsyncWithTimeOut<byte[]>(delegate
            {
                tcpSocket.Send(message);
                return GetResponse(tcpSocket);
            }, timeout);
        }
    }
}