using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Rpc.Net.Message.Filter;
using CTPPV5.Rpc.Net.Message.Serializer;

namespace CTPPV5.Rpc.Net.Message
{
    public class MessageHeader
    {
        public MessageHeader(
            string messageID,
            MessageVersion version,
            string identifier,
            byte[] filterCode,
            MessageFilterType filterType,
            MessageState state,
            ErrorCode errorCode,
            SerializeMode serializeMode,
            CommandCode commandCode,
            MessageType messageType)
        {
            this.MessageID = messageID;
            this.Version = version;
            this.State = state;
            this.FilterCode = filterCode;
            this.FilterType = filterType;
            this.ErrorCode = errorCode;
            this.SerializeMode = serializeMode;
            this.CommandCode = commandCode;
            this.Identifier = identifier;
            this.MessageType = messageType;
        }

        public MessageVersion Version { get; private set; }
        public string Identifier { get; private set; }
        public string MessageID { get; private set; }
        public CommandCode CommandCode { get; private set; }
        public byte[] FilterCode { get; set; }
        public MessageFilterType FilterType { get; set; }
        public MessageState State { get; private set; }
        public ErrorCode ErrorCode { get; private set; }
        public SerializeMode SerializeMode { get; set; }
        public MessageType MessageType { get; set; }
    }
}
