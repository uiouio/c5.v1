using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Metrics;

namespace CTPPV5.Infrastructure.Log
{
    public class Log4NetAdapter : CTPPV5.Infrastructure.Log.ILog
    {
        private log4net.ILog logImpl;
        private static Meter errorMetric = Metric.Meter("Errors", Unit.Errors, TimeUnit.Seconds);
        public Log4NetAdapter(log4net.ILog logImpl)
        {
            this.logImpl = logImpl;
        }

        public void Debug(object message)
        {
            logImpl.Debug(message);
        }

        public void Debug(string title, object message)
        {
            Debug(message);
        }

        public void Info(object message)
        {
            logImpl.Info(message);
        }

        public void Info(string title, object message)
        {
            Info(message);
        }

        public void Warn(object message)
        {
            logImpl.Warn(message);
        }

        public void Warn(string title, object message)
        {
            Warn(message);
        }

        public void Error(object message)
        {
            errorMetric.Mark();
            logImpl.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            errorMetric.Mark();
            logImpl.Error(message, exception);
        }

        public void Error(string title, object message, Exception exception)
        {
            Error(message, exception);
        }

        public void Fatal(object message)
        {
            logImpl.Fatal(message);
        }

        public void Fatal(object message, Exception exception)
        {
            logImpl.Fatal(message, exception);
        }

        public void Fatal(string title, object message, Exception exception)
        {
            Fatal(message, exception);
        }
    }
}
