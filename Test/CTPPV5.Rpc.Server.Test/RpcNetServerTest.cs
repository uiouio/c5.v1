using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using CTPPV5.Rpc.Net.Server;
using Mina.Transport.Socket;
using Mina.Core.Future;
using CTPPV5.TestLib;

namespace CTPPV5.Rpc.Server.Test
{
    [TestFixture]
    public class RpcNetServerTest
    {
        [Test]
        public void ServerStartUpTest()
        {
            var bootstrap = new ServerBootstrap(EndPointDispatcher.GetRandomPort());
            Assert.IsTrue(bootstrap.StartUp());
            var connector = new AsyncSocketConnector();
            var future = connector.Connect(bootstrap.EndPoint);
            Assert.IsTrue(future.Await(500));
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(1, bootstrap.Server.ManagedSessions.Count);
            Assert.IsTrue(future.Connected);
            Assert.IsNotNull(future.Session);
            bootstrap.Server.Dispose();
            connector.Dispose();
        }

        [Test]
        public void ServerDisconnectTest()
        {
            var bootstrap = new ServerBootstrap(EndPointDispatcher.GetRandomPort());
            Assert.IsTrue(bootstrap.StartUp());
            var connector = new AsyncSocketConnector();
            var future = connector.Connect(bootstrap.EndPoint);
            Assert.IsTrue(future.Await(500));
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(1, bootstrap.Server.ManagedSessions.Count);
            var closedFuture = future.Session.Close(true);
            Assert.IsTrue(closedFuture.Await(500));
            Assert.IsTrue(closedFuture.Closed);
            Assert.IsFalse(future.Session.Connected);
            System.Threading.Thread.Sleep(500);
            Assert.AreEqual(0, bootstrap.Server.ManagedSessions.Count);
            bootstrap.Server.Dispose();
            connector.Dispose();
        }
    }
}
