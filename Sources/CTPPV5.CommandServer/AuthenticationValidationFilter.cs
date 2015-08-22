using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Message.Serializer;
using Mina.Core.Filterchain;
using Mina.Core.Session;
using Mina.Core.Write;
using CTPPV5.Repository;
using CTPPV5.Models;
using CTPPV5.Models.CommandModel;
using CTPPV5.Rpc;
using CTPPV5.Rpc.Net.Server;
using CTPPV5.Rpc.Net.Message.Filter;
using CTPPV5.Rpc.Net;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.CommandServer
{
    public class AuthenticationValidationFilter : AbstractFilter
    {
        private IMetaRepository metaRepository;
        private SessionIdentifierManager identifierManager;
        public AuthenticationValidationFilter(SessionIdentifierManager identifierManager, IMetaRepository metaRepository)
        {
            this.metaRepository = metaRepository;
            this.identifierManager = identifierManager;
        }

        public override string Name
        {
            get { return "auth-validation"; }
        }

        public override void MessageReceived(INextFilter nextFilter, IoSession session, object message)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var commandMessage = message as DuplexMessage;
                if (commandMessage != null)
                {
                    if (commandMessage.Header.CommandCode == CommandCode.Authentication)
                    {
                        var school = metaRepository
                            .GetAllSchools()
                            .ByIdentifier(commandMessage.Header.Identifier);
                        if (school != null && school.UniqueToken.Equals(
                            commandMessage.GetContent<AuthInfo>().Mac, StringComparison.OrdinalIgnoreCase))
                        {
                            session.SetAttributeIfAbsent(KeyName.SESSION_IDENTIFIER, commandMessage.Header.Identifier);
                            identifierManager.SetSessonIdentifier(commandMessage.Header.Identifier, session);
                            nextFilter.FilterWrite(session,
                                new DefaultWriteRequest(
                                    DuplexMessage.CreateCallbackMessage(commandMessage)));
                        }
                        else
                        {
                            nextFilter.FilterWrite(session, 
                                new DefaultWriteRequest(
                                    DuplexMessage.CreateAckMessage(commandMessage, ErrorCode.AuthenticationFailed)));
                        }
                    }
                    else if (
                        commandMessage.Header.CommandCode == CommandCode.Register ||
                        commandMessage.Header.CommandCode == CommandCode.Heartbeat)
                    {
                        nextFilter.MessageReceived(session, message);
                    }
                    else
                    {
                        if (!session.ContainsAttribute(KeyName.SESSION_IDENTIFIER))
                        {
                            nextFilter.FilterWrite(session, new DefaultWriteRequest(
                                DuplexMessage.CreateAckMessage(commandMessage, ErrorCode.UnauthorizedCommand)));
                        }
                        else
                        {
                            var identifier = session.GetAttribute<string>(KeyName.SESSION_IDENTIFIER);
                            if (identifier.Equals(commandMessage.Header.Identifier))
                                nextFilter.MessageReceived(session, message);
                            else
                            {
                                nextFilter.FilterWrite(session, new DefaultWriteRequest(
                                    DuplexMessage.CreateAckMessage(commandMessage, ErrorCode.UnauthorizedCommand)));
                            }
                        }
                        
                    }
                }
                else
                {
                    nextFilter.FilterWrite(session, new DefaultWriteRequest(
                        DuplexMessage.CreateAckMessage(
                            Guid.NewGuid().ToByteArray().ToBase64(), 
                            MessageVersion.V1, 
                            CommandCode.UnAssigned, 
                            ErrorCode.IllegalCommandRequest)));
                }
            }
        }
    }
}
