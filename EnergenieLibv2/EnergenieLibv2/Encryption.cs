using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergenieLibv2
{
    internal static class Encryption
    {
        static byte[] _key; //= { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 }; // blank password
        internal static string keyAsBytes
        {
            get
            {
                return System.Text.Encoding.ASCII.GetString(_key);
            }
            set
            {
                _key = Encoding.ASCII.GetBytes(value);
            }
        }
        internal static byte DecryptState(byte[] encryptedState, byte[] challenge, int socket)
        {
            var socketState = (challenge[2] ^ ((_key[0] ^ (encryptedState[socket] - _key[1])) - challenge[3]));

            return Convert.ToByte(socketState & 0xFF);
        }

        internal static byte[] EncryptActionMsg(byte[] challenge, byte[] newState)
        {
            byte[] encryptedMsg = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                encryptedMsg[i] = Convert.ToByte(((_key[0] ^ (challenge[3] + (challenge[2] ^ newState[3 - i]))) + _key[1]) & 0xFF);
            }
            return encryptedMsg;
        }

        internal static byte[] CalcSolution(byte[] challenge)
        {
            var part1 = ((_key[2] ^ challenge[0]) * _key[0]) ^ (_key[4] << 8 | (_key[6])) ^ challenge[2];
            var part2 = ((_key[3] ^ challenge[1]) * _key[1]) ^ (_key[5] << 8 | (_key[7])) ^ challenge[3];

            byte[] result =
            {
                Convert.ToByte(part1 & 0xFF),
                Convert.ToByte(part1 >> 8 & 0xFF),
                Convert.ToByte(part2 & 0xFF),
                Convert.ToByte(part2 >> 8 & 0xFF),
            };
            return result;
        }
    }
}
