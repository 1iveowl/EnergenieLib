using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergenieLibv2.Interfaces
{
    public interface IEnergenie
    {
        Task<bool> GetSocketStateAsync(string key, int socket, string host, int port = 5000, int retries = 2, int timeout = 5);
        Task<bool> SetSocketStateAsync(string key,  int socket, bool state, string host, int port = 5000, int retries = 2, int timeout = 5);

    }
}
