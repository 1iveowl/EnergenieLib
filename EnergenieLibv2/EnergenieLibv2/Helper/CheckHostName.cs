using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EnergenieLibv2.Helper
{
    // Source: http://stackoverflow.com/questions/106179/regular-expression-to-match-dns-hostname-or-ip-address
    public static class CheckHostName
    {
        public static bool IsValidIpAdress(string host)
        {
            var ValidIpAddressRegex = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
            return (Regex.IsMatch(host, ValidIpAddressRegex));
        }

        public static bool IsValidIpDnsName(string host)
        {
            var ValidHostnameRegex = @"^(([a-zA-Z]|[a-zA-Z][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z]|[A-Za-z][A-Za-z0-9\-]*[A-Za-z0-9])$";
            return (Regex.IsMatch(host, ValidHostnameRegex));
        }
    }
}



 