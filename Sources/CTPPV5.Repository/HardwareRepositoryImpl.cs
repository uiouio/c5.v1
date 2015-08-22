using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Models;
using CTPPV5.Repository.Data;
using CTPPV5.Infrastructure.Log;

namespace CTPPV5.Repository
{
    public class HardwareRepositoryImpl : IHardwareRepository
    {
        public HardwareRepositoryImpl(IRepositoryConfigurator configurator)
        {
            this.ItemManager = configurator.ItemManager;
        }

        public SqlItemManager ItemManager { get; private set; }

        public HardwareCollection GetHardwares(int schoolId)
        {
            throw new NotImplementedException();
        }

        private ILog Log { get; set; }
    }
}
