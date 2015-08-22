using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using log4net;
using log4net.Config;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Extension;
using System.Runtime.CompilerServices;

namespace CTPPV5.Infrastructure
{
    public class ObjectHost
    {
        static IContainer host;
        static ILog log = LogManager.GetLogger(typeof(ObjectHost));

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool Setup(Autofac.Module[] modules) 
        {
            var setupOk = false;
            XmlConfigurator.Configure();
            try
            {
                var builder = new ContainerBuilder();
                modules.Foreach(m => builder.RegisterModule(m));
                host = builder.Build();
                setupOk = true;
            }
            catch (Exception ex)
            {
                log.Error(LogTitle.OBJECT_HOST_SETUP_ERROR, ex);
            }
            return setupOk;
        }

        public static IContainer Host { get { return host; } }
    }
}
