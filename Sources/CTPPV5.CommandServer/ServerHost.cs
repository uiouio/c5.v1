using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Models;
using CTPPV5.Repository;
using CTPPV5.Rpc.Net.Server;
using CTPPV5.Infrastructure;

namespace CTPPV5.CommandServer
{
    public partial class ServerHost : ServiceBase
    {
        public ServerHost()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var bootstrap = new ServerBootstrap();
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                bootstrap.AddFilters(scope.Resolve<AuthenticationValidationFilter>());
                bootstrap.StartUp(null, new Module[] { new ModelModule(), new RepositoryModule() });
            }
        }

        protected override void OnStop()
        {
        }
    }
}
