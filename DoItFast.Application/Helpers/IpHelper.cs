using System.Net;
using System.Net.Sockets;

namespace DoItFast.Application.Helpers
{
    public class IpHelper
    {
        /// <summary>
        /// Get ip address.
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in from ip in host.AddressList
                               where ip.AddressFamily == AddressFamily.InterNetwork
                               select ip)
            {
                return ip.ToString();
            }

            return string.Empty;
        }
    }
}
