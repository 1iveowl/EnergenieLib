using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnergenieLibv2;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            GetStateAsync();

            Console.WriteLine("Press any key to exit.");
            Console.ReadLine();
        }

        private static async void GetStateAsync()
        {
            string key = "key12345"; //must be 8 characters
            string hostIp = "192.168.0.1"; //Both ip adresses and...
            string hostDns = "energenie.local"; // ... dns host names can be used
            int socketNumber = 2; // must be 1, 2, 3 or 4 or the library will throw an exception.

            var energenie = new Energenie();

            //Get state of socket
            var status = await energenie.GetSocketStateAsync(key, socketNumber, hostDns, port:5000, retries:3, timeout:2);
            Console.WriteLine("Socket {0} is currently set to {1}", socketNumber, status);

            //Toggle socket
            status = !status;
            await energenie.SetSocketStateAsync(key, socketNumber, status, hostIp);

            //Get new state of socket
            status = await energenie.GetSocketStateAsync(key, socketNumber, hostIp);
            Console.WriteLine("Socket {0} is currently set to {1}", socketNumber, status);
        } 
    }
}
