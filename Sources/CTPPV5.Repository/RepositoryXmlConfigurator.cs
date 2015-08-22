using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Autofac;
using CTPPV5.Repository.Data;
using CTPPV5.Infrastructure;
using CTPPV5.Infrastructure.Cache;
using CTPPV5.Infrastructure.Consts;

namespace CTPPV5.Repository
{
    public class RepositoryXmlConfigurator : IRepositoryConfigurator
    {
        private bool isConfigured;
        public RepositoryXmlConfigurator()
        {
            ItemManager = new SqlItemManager();
        }

        public void Configure(bool throwIfItemExists = true)
        {
            Configure(Directory.GetCurrentDirectory() + @"\sqlitem.config", throwIfItemExists);
        }

        public void Configure(string path, bool throwIfItemExists = true)
        {
            if (!isConfigured)
            {
                using (var scope = ObjectHost.Host.BeginLifetimeScope())
                {
                    using (var reader = XmlReader.Create(path))
                    {
                        var connectionStringTemplate = string.Empty;
                        var readServer = string.Empty;
                        var writeServer = string.Empty;
                        var sqlItemName = string.Empty;
                        IConnectionBuilder builder = null;
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                switch (reader.Name.ToLower())
                                {
                                    case "mysql":
                                        {
                                            builder = scope.ResolveKeyed<IConnectionBuilder>(KeyName.MYSQL_DATACONNECTION_BUILDER);
                                            connectionStringTemplate = reader.GetAttribute("connectionString");
                                            readServer = reader.GetAttribute("readServer");
                                            writeServer = reader.GetAttribute("writeServer");
                                        }
                                        break;
                                    case "sqllite": builder = scope.ResolveKeyed<IConnectionBuilder>(KeyName.SQLLITE_DATACONNECTION_BUILDER);
                                        break;
                                    case "script": sqlItemName = reader.GetAttribute("name");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            if (reader.NodeType == XmlNodeType.Text
                                || reader.NodeType == XmlNodeType.CDATA)
                            {
                                if (!string.IsNullOrEmpty(sqlItemName))
                                {
                                    var sqlItem = new SqlItem(
                                        sqlItemName,
                                        reader.Value,
                                        readServer,
                                        writeServer,
                                        connectionStringTemplate,
                                        builder,
                                        scope.ResolveKeyed<ICache<string, object>>(KeyName.REPOSITORY_CACHE));

                                    if (!ItemManager.Add(sqlItemName, sqlItem))
                                    {
                                        if (throwIfItemExists)
                                            throw new SqlItemDuplicateException(sqlItem);
                                    }
                                }
                            }
                        }
                    }

                    isConfigured = true;
                }
            }
        }

        public SqlItemManager ItemManager { get; private set; }
    }
}
