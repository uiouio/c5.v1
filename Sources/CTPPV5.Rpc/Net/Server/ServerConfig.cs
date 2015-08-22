using CTPPV5.Infrastructure.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Server
{
    public class ServerConfig
    {
        private int? maxConnections;
        private int? readBufferSize;
        private int? sendBufferSize;
        private int? readerIdleTime;
        private int? writerIdleTime;
        private int? writeTimeout;
        private bool? keepAlive;

        public int MaxConnections
        {
            get
            {
                if (maxConnections == null)
                    maxConnections = AppConfig.GetValueFromAppSetting<int>("MaxConnections", 200);
                return maxConnections.Value;
            }
            set { maxConnections = value; }
        }

        public int ReadBufferSize
        {
            get
            {
                if (readBufferSize == null)
                    readBufferSize = AppConfig.GetValueFromAppSetting<int>("ReadBufferSize", 4096);
                return readBufferSize.Value;
            }
            set { readBufferSize = value; }
        }

        public int SendBufferSize
        {
            get
            {
                if (sendBufferSize == null)
                    sendBufferSize = AppConfig.GetValueFromAppSetting<int>("SendBufferSize", 4096);
                return sendBufferSize.Value;
            }
            set { sendBufferSize = value; }
        }

        public int ReaderIdleTime
        {
            get
            {
                if (readerIdleTime == null)
                    readerIdleTime = AppConfig.GetValueFromAppSetting<int>("ReaderIdleTime", 3 * 60);
                return readerIdleTime.Value;
            }
            set { readerIdleTime = value; }
        }

        public int WriterIdleTime
        {
            get
            {
                if (writerIdleTime == null)
                    writerIdleTime = AppConfig.GetValueFromAppSetting<int>("WriterIdleTime", 5 * 60);
                return writerIdleTime.Value;
            }
            set { writerIdleTime = value; }
        }

        public int WriteTimeout
        {
            get
            {
                if (writeTimeout == null)
                    writeTimeout = AppConfig.GetValueFromAppSetting<int>("WriteTimeout", 5);
                return writeTimeout.Value;
            }
            set { writeTimeout = value; }
        }

        public bool KeepAlive
        {
            get
            {
                if (keepAlive == null)
                    keepAlive = AppConfig.GetValueFromAppSetting<bool>("KeepAlive", true);
                return keepAlive.Value;
            }
            set { keepAlive = value; }
        }
    }
}
