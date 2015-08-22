using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Infrastructure.Collections.ProducerConsumer;
using CTPPV5.Infrastructure.Security;
using CTPPV5.Models;
using CTPPV5.Models.CommandModel;
using CTPPV5.Repository;
using CTPPV5.Rpc.Net.Command;
using CTPPV5.Rpc.Net.Message;
using CTPPV5.TestLib;
using Mina.Core.Session;
using Moq;

namespace CTPPV5.Rpc.Client.Test
{
    public class TestModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var schoolCollection = new SchoolCollection();
            schoolCollection.Add(new School
                {
                    Identifier = "098f6bcd4621d373cade4e832627b4f6",
                    ID = 1,
                    Name = "test"
                });
            var metaRepoMock = new Mock<IMetaRepository>();
            metaRepoMock.Setup(r => r.GetAllSchools()).Returns(schoolCollection);
            metaRepoMock.Setup(r => r.AddOrUpdateSchool(
                    It.IsAny<School>(),
                    It.IsAny<Action<bool>>()))
                .Callback<School, Action<bool>>((school, action) =>
                {
                    var model = schoolCollection.ByIdentifier(school.Identifier);
                    model.UniqueToken = school.UniqueToken;
                    new Task(() =>
                    {
                        System.Threading.Thread.Sleep(500);
                        action(true);
                    }).Start();
                });


            builder.Register(c => new TestIdentityProvider()).As<IIdentifierProvider>();
            builder.Register(c => new TestRsaKeyProvider()).As<ICryptoKeyProvider>();
            builder.Register(c => metaRepoMock.Object).As<IMetaRepository>().SingleInstance();
            builder.Register((c, p) => new SampleCommand(
                p.Named<IoSession>("session"),
                c.ResolveOptional<TimeoutNotifyProducerConsumer<AbstractAsyncCommand>>()
                )).Keyed<AbstractAsyncCommand>(CommandCode.Test);
        }
    }
}
