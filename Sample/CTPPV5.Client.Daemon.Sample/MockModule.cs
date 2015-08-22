using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Moq;
using CTPPV5.Repository;
using CTPPV5.Models;

namespace CTPPV5.Client.Daemon.Sample
{
    public class MockModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var hardwareRepoMock = new Mock<IHardwareRepository>();
            var hardwareCollections = new HardwareCollection();
            hardwareCollections.Add(new Hardware { Address = 1, Capacity = 100 });
            hardwareCollections.Add(new Hardware { Address = 2, Capacity = 100 });
            hardwareCollections.Add(new Hardware { Address = 3, Capacity = 100 });
            hardwareRepoMock.Setup(r => r.GetHardwares(0)).Returns(hardwareCollections);
            builder.Register(c => hardwareRepoMock.Object).As<IHardwareRepository>().SingleInstance();
        }
    }
}
