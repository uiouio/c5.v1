using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Util;
using log4net;
using CTPPV5.Infrastructure.Security;

namespace CTPPV5.Infrastructure.Module
{
    public class SecurityModule : Autofac.Module
    {
        static ILog log = LogManager.GetLogger(typeof(SecurityModule));
        protected override void Load(ContainerBuilder builder)
        {
            try
            {
                var file = AppDomain.CurrentDomain.BaseDirectory + "CTPPV5.Security.Impl.dll";
                var assembly = Assembly.LoadFrom(file);
                builder
                    .RegisterType(assembly
                        .GetType("CTPPV5.Security.Impl.RSACryptoKeyProvider"))
                    .As<ICryptoKeyProvider>()
                    .SingleInstance();
                builder
                    .RegisterType(assembly
                        .GetType("CTPPV5.Security.Impl.IdentifierProvider"))
                    .As<IIdentifierProvider>()
                    .SingleInstance();
            }
            catch (Exception ex)
            {
                log.Error(LogTitle.SECURITY_MODULE_LOAD_ERROR, ex);
            }
        }
    }
}
