using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using EnergenieLibv2.Interfaces;
using EnergenieLibv2.Extensions;
using static EnergenieLibv2.Encryption;
using static EnergenieLibv2.Helper.CheckHostName;
using static EnergenieLibv2.Helper.RunAsyncHelper;
using static EnergenieLibv2.Helper.TcpSocketHelper;

namespace EnergenieLibv2
{
    
    public class Energenie : IEnergenie
    {
        private const byte IsOn = 0x41;
        private const byte IsOff = 0x82;

        private const byte SwitchOn = 0x01;
        private const byte SwitchOff = 0x02;
        private const byte NoSwitching = 0x04;

        private byte[] Handshake = { 0x11 };

        private byte[] buffer = new byte[1024];

        #region Constructor
        public Energenie()
        {

        }
        #endregion

        public async Task<bool> GetSocketStateAsync(string key, int socket, string host, int port = 5000, int retries = 3, int timeout = 3)
        {
            ExitOnInvalidSocketNumber(socket);
            ExitOnInvalidKey(key);

            keyAsBytes = key;

            return await RunAsyncWithRetries<bool>(async delegate
            {
                using (var tcpSocket = await GetTcpSocket(host, port, timeout))
                {
                    var challenge = await tcpSocket.SendReceiveAsync(Handshake, timeout);
                    var solution = CalcSolution(challenge);
                    var encryptedStatus = await tcpSocket.SendReceiveAsync(solution, timeout);

                    tcpSocket.Close();

                    return DecryptState(encryptedStatus, challenge, socket) == IsOn;
                }
            }, retries);
        }

        public async Task<bool> SetSocketStateAsync(string key, int socket, bool state, string host, int port = 5000, int retries = 3, int timeout = 3)
        {
            ExitOnInvalidSocketNumber(socket);
            ExitOnInvalidKey(key);

            keyAsBytes = key;

            byte[] newState = { NoSwitching, NoSwitching, NoSwitching, NoSwitching };
            newState[socket - 1] = ConvertFromBoolToEnergenieState(state);

            keyAsBytes = key;

            return await RunAsyncWithRetries<bool>(async delegate
            {
                using (var tcpSocket = await GetTcpSocket(host, port, timeout))
                {
                    var challenge = await tcpSocket.SendReceiveAsync(Handshake, timeout);
                    var solution = CalcSolution(challenge);
                    await tcpSocket.SendReceiveAsync(solution, timeout);

                    var controlMsg = EncryptActionMsg(challenge, newState);
                    var sendControlMsg = await tcpSocket.SendReceiveAsync(controlMsg, timeout);
                    tcpSocket.Close();
                    return true;
                }
            }, retries);
        }

        private void ExitOnInvalidSocketNumber(int socket)
        {
            if ((socket > 0) && (socket <= 4)) return;
                else throw new ArgumentOutOfRangeException("Socket number must be 1, 2, 3 or 4. Current socket is {0}", socket.ToString());
        }

        private void ExitOnInvalidKey(string key)
        {
            if (key.Length != 8) throw new ArgumentOutOfRangeException("Key must be 8 characters. Current key has {0} characters", key.Length.ToString());
        }


        private byte ConvertFromBoolToEnergenieState(bool state)
        {
            switch (state)
            {
                case true: return SwitchOn;
                case false: return SwitchOff;
                default: return NoSwitching;
            }
        }
    }
}
