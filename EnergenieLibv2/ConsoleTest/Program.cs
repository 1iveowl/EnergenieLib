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
            string password = "password";
            string hostIp = "192.168.0.1"; 
            string hostDns = "energenie.local";
            int socketNumber = 2;

            var energenie = new Energenie();

            //Get state of socket
            var status = await energenie.GetSocketStateAsync(password, socketNumber, hostDns, port:5000, retries:3, timeout:2);
            Console.WriteLine("Socket {0} is currently set to {1}", socketNumber, status);

            //Toggle socket
            status = !status;
            await energenie.SetSocketStateAsync(password, socketNumber, status, hostIp);

            //Get new state of socket
            status = await energenie.GetSocketStateAsync(password, socketNumber, hostIp);
            Console.WriteLine("Socket {0} is currently set to {1}", socketNumber, status);
        } 
    }
}
