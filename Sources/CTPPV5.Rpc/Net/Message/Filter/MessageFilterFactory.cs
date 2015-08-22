using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Extension;

namespace CTPPV5.Rpc.Net.Message.Filter
{
    public class MessageFilterFactory
    {
        private Dictionary<byte, IMessageFilter> compressionMap = new Dictionary<byte, IMessageFilter>();
        private Dictionary<byte, IMessageFilter> cryptoMap = new Dictionary<byte, IMessageFilter>();
        private Dictionary<byte, IMessageFilter> checksumMap = new Dictionary<byte, IMessageFilter>();
        private Dictionary<MessageFilterType, Dictionary<byte, IMessageFilter>> filterMap = new Dictionary<MessageFilterType, Dictionary<byte, IMessageFilter>>();
        private Dictionary<MessageFilterType, byte> posMap = new Dictionary<MessageFilterType, byte>();

        public MessageFilterFactory()
        {
            //one byte contains four filtertypes
            //two bits contain four filters
            //00 00 00 00
            //1st 00 - checksum
            //2nd 00 - gzip
            //3rd 00 - crypto

            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                checksumMap.Add(ChecksumMode.Crc32.ToByte(), scope.ResolveKeyed<IMessageFilter>(ChecksumMode.Crc32));
                compressionMap.Add(ZipMode.GZip.ToByte(), scope.ResolveKeyed<IMessageFilter>(ZipMode.GZip));
                cryptoMap.Add(CryptoMode.RSA.ToByte(), scope.ResolveKeyed<IMessageFilter>(CryptoMode.RSA));
            }

            filterMap.Add(MessageFilterType.Checksum, checksumMap);
            filterMap.Add(MessageFilterType.Compression, compressionMap);
            filterMap.Add(MessageFilterType.Crypto, cryptoMap);

            posMap.Add(MessageFilterType.Checksum, 3);
            posMap.Add(MessageFilterType.Compression, 12);
            posMap.Add(MessageFilterType.Crypto, 48);
        }

        public IMessageFilter CreateFilter(byte key, MessageFilterType type)
        {
            IMessageFilter filter = null;
            Dictionary<byte, IMessageFilter> map = null;
            if (filterMap.TryGetValue(type, out map))
            {
                var pos = posMap[type];
                map.TryGetValue((byte)(key & pos), out filter);
            }
            return filter ?? Empty.Instance;
        }

        public static byte[] CreateDefaultFilterCode()
        {
            return new byte[2];
        }
    }
}
