using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Rpc.Net.Server;
using Mina.Core.Session;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Rpc.Net.Message.Filter;

namespace CTPPV5.CommandServer.Command
{
    public class ListConnectors : AbstractCommandExecutor
    {
        private SessionIdentifierManager identifierManager;
        public ListConnectors(SessionIdentifierManager identifierManager)
        {
            this.identifierManager = identifierManager;
        }

        protected override DuplexMessage DoExecute(DuplexMessage commandMessage)
        {
            IoSession session = null;
            DuplexMessage resultMessage = null;
            if (identifierManager.TryGetSessonIdByIdentifier(commandMessage.Header.Identifier, out session))
            {
                var connectors = session.Service
                    .ManagedSessions
                    .Values
                    .Where(s => s.Id != session.Id)
                    .Select(s => s.GetAttribute<string>(KeyName.SESSION_IDENTIFIER))
                    .ToList();
                resultMessage = DuplexMessage.CreateCallbackMessage(commandMessage, connectors);
                resultMessage.Header.FilterType |= MessageFilterType.Crypto;
                resultMessage.Header.SerializeMode = Rpc.Net.Message.Serializer.SerializeMode.Json;
            }
            else
            {
                resultMessage = DuplexMessage.CreateCallbackMessage(commandMessage);
            }
            return resultMessage;
        }
    }
}
