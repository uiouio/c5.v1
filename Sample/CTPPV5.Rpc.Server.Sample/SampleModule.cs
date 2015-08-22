using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.TestLib;
using CTPPV5.Models;
using CTPPV5.Repository;

namespace CTPPV5.Rpc.Server.Sample
{
    public class SampleModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new TestRsaKeyProvider()).As<ICryptoKeyProvider>();
            builder.Register(c => new SampleCommandExecutor()).Keyed<ICommandExecutor<DuplexMessage>>(CommandCode.Test);
            builder.Register(c => new TestIdentityProvider()).As<IIdentifierProvider>();
            builder.Register(c => new MetaTestRepo()).As<IMetaRepository>().SingleInstance();
        }

        public class MetaTestRepo : IMetaRepository
        {
            private SchoolCollection schoolCollection;
            public MetaTestRepo()
            {
                schoolCollection = new SchoolCollection();
                schoolCollection.Add(new School
                {
                    Identifier = "098f6bcd4621d373cade4e832627b4f6",
                    ID = 1,
                    Name = "test"
                });
            }

            #region IMetaRepository Members

            public void AddOrUpdateSchool(School school, Action<bool> callback)
            {
                System.Threading.ThreadPool.QueueUserWorkItem((_) => callback(true), null);
            }

            public SchoolCollection GetAllSchools()
            {
                return schoolCollection;
            }

            #endregion
        }
    }
}
