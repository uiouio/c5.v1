using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Rpc.Net.Client
{
    public class ConnectionConfig
    {
        private const int DEFAULT_CONNECT_TIMEOUT = 5;
        private const bool SHOULD_KEEP_ALIVE = true;
        private static Regex timeoutRegex = new Regex("timeout=(?<timeout>\\d+)", RegexOptions.IgnoreCase);
        private static Regex keepAliveRegex = new Regex("keepalive=(?<keepalive>true|false)", RegexOptions.IgnoreCase);

        private ConnectionConfig() 
            :this(null){ }

        public ConnectionConfig(IPEndPoint endpoint)
        {
            this.EndPoint = endpoint;
            KeepAlive = SHOULD_KEEP_ALIVE;
            Timeout = DEFAULT_CONNECT_TIMEOUT;
        }

        public IPEndPoint EndPoint { get; private set; }
        public int Timeout { get; private set; }
        public bool KeepAlive { get; private set; }

        public static bool TryParse(string connectionString, out ConnectionConfig connectionItem)
        {
            var parsedOk = false;
            connectionItem = new ConnectionConfig();
            if (string.IsNullOrEmpty(connectionString)) return false;
            var split = connectionString.Split(';');
            var endpointItems = split[0].Split(':');
            if (endpointItems.Length == 2)
            {
                IPAddress addr = null;
                if (IPAddress.TryParse(endpointItems[0], out addr)
                    && endpointItems[1].IsNumeric())
                {
                    connectionItem.EndPoint = new IPEndPoint(addr, Convert.ToInt32(endpointItems[1]));
                    parsedOk = true;
                }
            }
            if (parsedOk)
            {
                var match = timeoutRegex.Match(connectionString);
                if (match.Success)
                    connectionItem.Timeout = Convert.ToInt32(match.Groups["timeout"].Value);
                match = keepAliveRegex.Match(connectionString);
                if (match.Success)
                    connectionItem.KeepAlive = Convert.ToBoolean(match.Groups["keepalive"].Value);
            }
            return parsedOk;
        }
    }
}
