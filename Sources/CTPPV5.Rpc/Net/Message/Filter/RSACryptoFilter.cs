using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Autofac;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure.Util;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Infrastructure.Log;

namespace CTPPV5.Rpc.Net.Message.Filter
{
    public class RSACryptoFilter : IMessageFilter
    {
        const int CRYPTO_KEY_SIZE = 1024;
        
        public FilterResult In(IMessageDataContainer container)
        {
            var result = new FilterResult { OK = true };
            try
            {
                using (var scope = ObjectHost.Host.BeginLifetimeScope())
                {
                    var cypher = container.Take();
                    var keyId = container.Take().ToHex();
                    var keyProvider = scope.Resolve<ICryptoKeyProvider>();
                    var privateKey = keyProvider.GetPrivateKey(keyId);
                    if (privateKey != null && privateKey.Length > 0)
                    {
                        container.Push(new RSACrypto().Decrypt(privateKey, cypher));
                    }
                    else result = new FilterResult { Error = ErrorCode.CryptoKeyReadFailed };
                }
            }
            catch (Exception ex)
            {
                result = new FilterResult { Error = ErrorCode.DecryptFailed };
                Log.Error(result.Error.ToString(), ex);
                
            }
            return result;
        }

        public FilterResult Out(MessageHeader header, IMessageDataContainer container)
        {
            var result = new FilterResult { OK = true };
            try
            {
                using (var scope = ObjectHost.Host.BeginLifetimeScope())
                {
                    var plainText = container.Take();
                    var keyId = container.Take().ToHex();
                    var keyProvider = scope.Resolve<ICryptoKeyProvider>();
                    var pubKey = keyProvider.GetPublicKey(keyId);
                    if (pubKey != null && pubKey.Length > 0)
                    {
                        container.Push(new RSACrypto().Encrypt(pubKey, plainText));
                    }
                    else result = new FilterResult { Error = ErrorCode.CryptoKeyReadFailed };
                }
            }
            catch (Exception ex)
            {
                result = new FilterResult { Error = ErrorCode.EncryptFailed };
                Log.Error(result.Error.ToString(), ex);

            }
            return result;
        }

        private ILog Log { get; set; }
    }
}
