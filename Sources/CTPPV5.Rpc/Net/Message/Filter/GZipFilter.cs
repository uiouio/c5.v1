using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Extension;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Log;

namespace CTPPV5.Rpc.Net.Message.Filter
{
    public class GZipFilter : IMessageFilter
    {
        const int ZIPPING_SIZE = 8 * 1024;
        public FilterResult In(IMessageDataContainer container)
        {
            var result = new FilterResult { OK = true };
            try
            {
                var data = container.Take();
                if (data.Length > 0)
                    container.Push(data.Unzip());
            }
            catch (Exception ex)
            {
                result = new FilterResult { Error = ErrorCode.UnzipFailed };
                Log.Error(result.Error.ToString(), ex);
            }
            return result;
        }

        public FilterResult Out(MessageHeader header, IMessageDataContainer container)
        {
            var body = container.Take();
            if (body.Length >= ZIPPING_SIZE)
            {
                container.Push(body.Zip());
                header.FilterType |= MessageFilterType.Compression;
            }
            else
            {
                if (body.Length != 0) container.Push(body);
                if ((header.FilterType & MessageFilterType.Compression) == MessageFilterType.Compression)
                    header.FilterType ^= MessageFilterType.Compression;
            }
            return new FilterResult { OK = true };
        }

        private ILog Log { get; set; }
    }
}
