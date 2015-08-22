using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Cache;
using Autofac;
using CTPPV5.Repository.Data;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Models;

namespace CTPPV5.Repository
{
    public class RepositoryModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ModelModule());
            builder.Register(c => new LocalCache<string, object>(500)).Keyed<ICache<string, object>>(KeyName.REPOSITORY_CACHE).SingleInstance();
            builder
                .Register(c => new RepositoryXmlConfigurator())
                .As<IRepositoryConfigurator>()
                .SingleInstance()
                .OnActivated(a => a.Instance.Configure());
            builder.Register(c => new MySqlConnectionBuilder()).Keyed<IConnectionBuilder>(KeyName.MYSQL_DATACONNECTION_BUILDER);
            builder.Register(c => new SqlliteConnectionBuilder()).Keyed<IConnectionBuilder>(KeyName.SQLLITE_DATACONNECTION_BUILDER);
            builder.Register(c => new MetaRepositoryImpl(c.Resolve<IRepositoryConfigurator>())).As<IMetaRepository>().SingleInstance();
            builder.Register(c => new HardwareRepositoryImpl(c.Resolve<IRepositoryConfigurator>())).As<IHardwareRepository>().SingleInstance();
        }
    }
}
