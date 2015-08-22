using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Infrastructure.Log
{
    public interface ILog
    {
        void Debug(object message);
        void Debug(string title, object message);
        void Info(object message);
        void Info(string title, object message);
        void Warn(object message);
        void Warn(string title, object message);
        void Error(object message);
        void Error(object message, Exception exception);
        void Error(string title, object message, Exception exception);
        void Fatal(object message);
        void Fatal(object message, Exception exception);
        void Fatal(string title, object message, Exception exception);
    }
}
