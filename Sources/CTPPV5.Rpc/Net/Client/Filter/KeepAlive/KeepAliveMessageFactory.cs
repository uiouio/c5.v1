using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Filter.KeepAlive;
using Mina.Core.Session;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Rpc.Net.Client.Filter.KeepAlive
{
    public class KeepAliveMessageFactory : IKeepAliveMessageFactory
    {
        public bool IsRequest(IoSession session, object message)
        {
            var isRequest = false;
            var heartbeat = message as DuplexMessage;
            if (heartbeat != null) isRequest = heartbeat.Header.CommandCode == CommandCode.Heartbeat;
            return isRequest;
        }

        public bool IsResponse(IoSession session, object message)
        {
            var isResponse = false;
            var heartbeat = message as DuplexMessage;
            if (heartbeat != null) isResponse = heartbeat.Header.MessageType == MessageType.Heartbeat;
            return isResponse;
        }

        public object GetRequest(IoSession session)
        {
            return DuplexMessage.CreateHeartbeat();
        }

        public object GetResponse(IoSession session, object request)
        {
            return null;
        }
    }
}
