using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Infrastructure.Consts;
using CTPPV5.Repository.Data;

namespace CTPPV5.Repository
{
    public class SqlItemDuplicateException : Exception
    {
        public SqlItemDuplicateException(SqlItem item)
            : base(string.Format(ExceptionMessage.SQLITEM_DUPLICATE_EXCEPTION, item.Name))
        { }
    }
}
