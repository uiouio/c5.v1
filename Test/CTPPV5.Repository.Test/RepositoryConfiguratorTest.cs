using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Repository;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using Autofac;
using CTPPV5.Infrastructure;

namespace CTPPV5.Repository.Test
{
    [TestFixture]
    public class RepositoryConfiguratorTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            ObjectHost.Setup(new Module[] { new RepositoryModule() });
        }

        [Test]
        public void TestLoad()
        {
            var configurator = new RepositoryXmlConfigurator();
            configurator.Configure(Directory.GetCurrentDirectory() + @"\sqlitem2.config", false);
            var manager = configurator.ItemManager;
            Assert.AreEqual(7, manager.ItemCount);
            var item = manager.Get("GetSchoolsByAreaID");
            Assert.AreEqual("GetSchoolsByAreaID", item.Name);
            Assert.AreEqual("select * from experiment where id in @id", RemoveWhitespace(item.Sql));
            Assert.Throws<MySqlException>(() => item.OpenConnection("aa", "22"));
            item = manager.Get("ABTest1");
            Assert.AreEqual("ABTest1", item.Name);
            Assert.AreEqual("select * from experiment where id in @id", item.Sql);
            item = manager.Get("ABTest6");
            Assert.AreEqual("ABTest6", item.Name);
            Assert.AreEqual("select count(1) from experiment where id in @id", RemoveWhitespace(item.Sql));
        }

        [Test]
        public void TestDuplicateIgnore()
        {
            var configurator = new RepositoryXmlConfigurator();
            configurator.Configure(Directory.GetCurrentDirectory() + @"\sqlitem.config", false);
            var manager = configurator.ItemManager;
            Assert.AreEqual(4, manager.ItemCount);
            var item = manager.Get("GetSchoolsByAreaID");
            Assert.AreEqual("GetSchoolsByAreaID", item.Name);
            Assert.AreEqual("select * from experiment where id in @id", RemoveWhitespace(item.Sql));
            Assert.Throws<MySqlException>(() => item.OpenConnection("aa", "22"));
            item = manager.Get("ABTest1");
            Assert.AreEqual("ABTest1", item.Name);
            Assert.AreEqual("select * from experiment where id in @id", item.Sql);
            item = manager.Get("ABTest3");
            Assert.AreEqual("ABTest3", item.Name);
            Assert.AreEqual("select * from experiment where id in @id", RemoveWhitespace(item.Sql));
        }

        [Test]
        public void TestDuplicateThrow()
        {
            var configurator = new RepositoryXmlConfigurator();
            Assert.Throws<SqlItemDuplicateException>(() => configurator.Configure());
        }

        public string RemoveWhitespace(string input)
        {
            return input
                .Replace("\t", string.Empty)
                .Replace("\r", string.Empty)
                .Replace("\n", string.Empty);
        }
    }
}
