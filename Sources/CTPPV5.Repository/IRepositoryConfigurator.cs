using CTPPV5.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTPPV5.Repository
{
    public interface IRepositoryConfigurator
    {
        SqlItemManager ItemManager { get; }
        void Configure(bool throwIfItemExists);
        void Configure(string path, bool throwIfItemExists);
    }
}
