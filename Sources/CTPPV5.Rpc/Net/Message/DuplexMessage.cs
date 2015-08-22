using CTPPV5.Infrastructure;
using CTPPV5.Rpc.Net.Message.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Rpc.Net.Message.Filter;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure.Security;

namespace CTPPV5.Rpc.Net.Message
{
    public class DuplexMessage : IMessage, IDisposable
    {
        private object content;
        private byte[] contentBinary;
        private DuplexMessage(MessageHeader header, object content)
        {
            this.Header = header;
            this.content = content;
        }

        private DuplexMessage(MessageHeader header, byte[] contentBinary)
        {
            this.Header = header;
            this.contentBinary = contentBinary;
        }

        public MessageHeader Header { get; private set; }

        public T GetContent<T>()
        {
            if (content != null && content.GetType() == typeof(T)) return (T)content;
            if (content != null && typeof(T) == typeof(object)) return (T)content;
            if (typeof(T) == typeof(byte[])) return (T)(object)contentBinary;
            if (contentBinary == null || contentBinary.Length == 0) return default(T);
            if (content == null)
            {
                using (var scope = ObjectHost.Host.BeginLifetimeScope())
                {
                    content = scope.ResolveKeyed<ISerializer>(Header.SerializeMode).DeSerialize<T>(contentBinary);
                    contentBinary = null;
                }
            }
            return (T)content;
        }

        public byte[] GetContentBinary()
        {
            if (contentBinary != null && contentBinary.Length > 0) return contentBinary;
            if (content == null) return new byte[0];
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                contentBinary = scope.ResolveKeyed<ISerializer>(Header.SerializeMode).Serialize(content);
            }
            return contentBinary;
        }

        public static DuplexMessage CreateCommandMessage(
            string messageId, MessageVersion version, CommandCode commandCode)
        {
            return CreateCommandMessage(
                messageId, 
                version, 
                commandCode, 
                MessageFilterType.Checksum, 
                MessageFilterFactory.CreateDefaultFilterCode(), 
                SerializeMode.None, 
                null);
        }

        public static DuplexMessage CreateCommandMessage(
            string messageId, 
            MessageVersion version, 
            CommandCode commandCode, 
            MessageFilterType filterType,
            byte[] filterCode, 
            SerializeMode serializeMode, 
            object content)
        {
            var identifier = ObjectHost.Host.Resolve<IIdentifierProvider>().GetIdentifier();
            if (content != null)
            {
                return CreateCommandMessage(
                    messageId,
                    version,
                    identifier,
                    filterType,
                    filterCode,
                    MessageState.Success,
                    ErrorCode.NoError,
                    serializeMode,
                    commandCode,
                    content); 
            }
            else
            {
                return CreateCommandMessage(
                    messageId,
                    version,
                    identifier,
                    MessageFilterType.Checksum,
                    MessageFilterFactory.CreateDefaultFilterCode(),
                    MessageState.Success,
                    ErrorCode.NoError,
                    SerializeMode.None,
                    commandCode);
            }
        }

        public static DuplexMessage CreateCommandMessage(
            MessageVersion version, string identifier, ErrorCode errorCode, CommandCode commandCode)
        {
            return CreateCommandMessage(
                Convert.ToBase64String(Guid.NewGuid().ToByteArray()), version, identifier, errorCode, commandCode);
        }

        public static DuplexMessage CreateCommandMessage(
            string messageId, MessageVersion version, string identifier, ErrorCode errorCode, CommandCode commandCode)
        {
            return CreateCommandMessage(
                messageId,
                version,
                identifier,
                MessageFilterType.Checksum,
                MessageFilterFactory.CreateDefaultFilterCode(),
                MessageState.Fail,
                errorCode,
                SerializeMode.None,
                commandCode);
        }

        public static DuplexMessage CreateCommandMessage(
            string messageId,
            MessageVersion version,
            string identifier,
            MessageFilterType filterType,
            byte[] filterCode,
            MessageState state,
            ErrorCode errorCode,
            SerializeMode serializeMode,
            CommandCode commandCode,
            object content = null)
        {
            if (content != null)
            {
                return new DuplexMessage(new MessageHeader(
                    messageId,
                    version,
                    identifier,
                    filterCode,
                    filterType,
                    state,
                    errorCode,
                    serializeMode,
                    commandCode,
                    MessageType.Command), content);
            }
            else 
            {
                return new DuplexMessage(new MessageHeader(
                    messageId,
                    version,
                    identifier,
                    MessageFilterFactory.CreateDefaultFilterCode(),
                    MessageFilterType.Checksum,
                    state,
                    errorCode,
                    SerializeMode.None,
                    commandCode,
                    MessageType.Command), null);
            }
        }

        public static DuplexMessage CreateHeartbeat()
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                return new DuplexMessage(new MessageHeader(
                   Guid.NewGuid().ToByteArray().ToBase64(),
                   MessageVersion.V1,
                   scope.Resolve<IIdentifierProvider>().GetIdentifier(),
                   MessageFilterFactory.CreateDefaultFilterCode(),
                   MessageFilterType.Checksum,
                   MessageState.Success,
                   ErrorCode.NoError,
                   SerializeMode.None,
                   CommandCode.Heartbeat,
                   MessageType.Heartbeat), null); 
            }
        }

        public static DuplexMessage CreateAckMessage(DuplexMessage commandMessage)
        {
            return new DuplexMessage(new MessageHeader(
                commandMessage.Header.MessageID,
                commandMessage.Header.Version,
                commandMessage.Header.Identifier,
                MessageFilterFactory.CreateDefaultFilterCode(),
                MessageFilterType.Checksum,
                MessageState.Success,
                ErrorCode.NoError,
                SerializeMode.None,
                commandMessage.Header.CommandCode,
                MessageType.CommandAck), null);
        }

        public static DuplexMessage CreateAckMessage(DuplexMessage commandMessage, ErrorCode errorCode)
        {
            return CreateAckMessage(
                commandMessage.Header.MessageID,
                commandMessage.Header.Version,
                commandMessage.Header.Identifier,
                commandMessage.Header.CommandCode,
                errorCode);
        }

        public static DuplexMessage CreateAckMessage(string messageId, MessageVersion version, CommandCode commandCode, ErrorCode errorCode)
        {
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                return CreateAckMessage(messageId, version, scope.Resolve<IIdentifierProvider>().GetIdentifier(), commandCode, errorCode);
            }
        }

        public static DuplexMessage CreateAckMessage(
            string messageId, string identifier, ErrorCode errorCode)
        {
            return CreateAckMessage(messageId, MessageVersion.V1, identifier, CommandCode.UnAssigned, errorCode);
        }

        public static DuplexMessage CreateAckMessage(
            string messageId, MessageVersion version, string identifier, CommandCode commandCode, ErrorCode errorCode)
        {
            return new DuplexMessage(new MessageHeader(
                messageId,
                version,
                identifier,
                MessageFilterFactory.CreateDefaultFilterCode(),
                MessageFilterType.Checksum,
                MessageState.Fail,
                errorCode,
                SerializeMode.None,
                commandCode,
                MessageType.CommandAck), null);
        }

        public static DuplexMessage CreateCallbackMessage(DuplexMessage commandMessage, object content = null)
        {
            if (content != null)
            {
                return new DuplexMessage(new MessageHeader(
                    commandMessage.Header.MessageID,
                    commandMessage.Header.Version,
                    commandMessage.Header.Identifier,
                    commandMessage.Header.FilterCode,
                    commandMessage.Header.FilterType | MessageFilterType.Compression,
                    MessageState.Success,
                    ErrorCode.NoError,
                    commandMessage.Header.SerializeMode,
                    commandMessage.Header.CommandCode,
                    MessageType.Callback), content);
            }
            else
            {
                return new DuplexMessage(new MessageHeader(
                    commandMessage.Header.MessageID,
                    commandMessage.Header.Version,
                    commandMessage.Header.Identifier,
                    MessageFilterFactory.CreateDefaultFilterCode(),
                    MessageFilterType.Checksum,
                    MessageState.Success,
                    ErrorCode.NoError,
                    SerializeMode.None,
                    commandMessage.Header.CommandCode,
                    MessageType.Callback), content);
            }
        }

        public static DuplexMessage CreateCallbackMessage(DuplexMessage commandMessage, ErrorCode errorCode)
        {
            return new DuplexMessage(new MessageHeader(
                commandMessage.Header.MessageID,
                commandMessage.Header.Version,
                commandMessage.Header.Identifier,
                MessageFilterFactory.CreateDefaultFilterCode(),
                MessageFilterType.Checksum,
                MessageState.Fail,
                errorCode,
                SerializeMode.None,
                commandMessage.Header.CommandCode,
                MessageType.Callback), null);
        }

        public static DuplexMessage CreateMessage(MessageHeader header, object content)
        {
            return new DuplexMessage(header, content);
        }

        public static DuplexMessage CreateMessage(MessageHeader header, byte[] contentBinary)
        {
            return new DuplexMessage(header, contentBinary);
        }

        public void Dispose()
        {
            contentBinary = null;
            content = null;
        }
    }
}
