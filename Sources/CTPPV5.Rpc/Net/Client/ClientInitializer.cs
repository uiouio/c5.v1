using Autofac;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Rpc.Net.Client
{
    public class ClientInitializer
    {
        public bool Init(params Autofac.Module[] modules)
        {
            return ObjectHost.Setup(
                new Module[] 
                { 
                    new RpcNetModule(),
                    new ClientModule()
                }.Concat(modules));
        }
    }
}
