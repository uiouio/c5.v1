using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using CTPPV5.Models;
using CTPPV5.Repository.Data;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Infrastructure.Log;

namespace CTPPV5.Repository
{
    public class MetaRepositoryImpl : IMetaRepository
    {
        private int SCHOOL_COLLECTION_CACHE_TIME_MINUTES = 2;
        public MetaRepositoryImpl(IRepositoryConfigurator configurator)
        {
            this.ItemManager = configurator.ItemManager;
        }

        public SqlItemManager ItemManager { get; private set; }

        public void AddOrUpdateSchool(School school, Action<bool> callback)
        {
 
        }

        public SchoolCollection GetAllSchools()
        {
            var sqlItem = ItemManager.Get(SqlName.AllSchools);
            return sqlItem.Cache.GetOrAdd(string.Format("{0}-{1}", KeyName.META_DATABASE, SqlName.AllSchools), () =>
            {
                SchoolCollection collection = new SchoolCollection();
                try
                {
                    using (var connection = sqlItem.OpenReadConnection(KeyName.META_DATABASE))
                    {
                        collection = Mapper.Map<IEnumerable<School>, SchoolCollection>(connection.Query<School>(sqlItem.Sql));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(LogTitle.DB_ERROR, ex);
                }
                return collection;
            }, TimeSpan.FromMinutes(SCHOOL_COLLECTION_CACHE_TIME_MINUTES)) as SchoolCollection;
        }

        private ILog Log { get; set; }
    }
}
