using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CTPPV5.Client.Winform.Views.Modules
{
    public interface IToolStripModule : IModule
    {
        ToolStripButton NavButton { get; }
    }
}
