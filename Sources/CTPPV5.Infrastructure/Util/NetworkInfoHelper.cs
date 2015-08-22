using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Util
{
    public class NetworkInfoHelper
    {
        static string macAddress = string.Empty;
        public static string GetMacAddr()
        {
            if (!string.IsNullOrEmpty(macAddress)) return macAddress;

            int foundCount = 0;
            string macAddr = string.Empty;
            var adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var adapter in adapters)
            {
                foundCount++;
                var addr = adapter.GetPhysicalAddress().ToString();
                if (addr.Equals("本地连接")) macAddr = addr;
            }
            if (string.IsNullOrEmpty(macAddr)) 
                macAddr = foundCount != 0 ? adapters[0].GetPhysicalAddress().ToString() : string.Empty;
            
            macAddress = macAddr;

            return macAddress;
        }
    }
}
