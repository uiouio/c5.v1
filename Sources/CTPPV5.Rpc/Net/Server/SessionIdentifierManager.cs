using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mina.Core.Session;

namespace CTPPV5.Rpc.Net.Server
{
    public class SessionIdentifierManager
    {
        private ConcurrentDictionary<string, IoSession> identifierMap = new ConcurrentDictionary<string, IoSession>();
        public bool TryGetSessonIdByIdentifier(string identifier, out IoSession session)
        {
            session = null;
            if (string.IsNullOrEmpty(identifier)) return false;
            return identifierMap.TryGetValue(identifier, out session);
        }

        public void SetSessonIdentifier(string identifier, IoSession session)
        {
            if (!string.IsNullOrEmpty(identifier))
                identifierMap[identifier] = session;
        }

        public void RemoveSessionId(string identifier)
        {
            IoSession session = null;
            if (!string.IsNullOrEmpty(identifier))
                identifierMap.TryRemove(identifier, out session);
        }
    }
}
