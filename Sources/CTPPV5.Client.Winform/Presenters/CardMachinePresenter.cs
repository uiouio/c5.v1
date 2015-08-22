using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Client.Winform.Views.Modules;
using WinFormsMvp;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface ICardMachinePresenter : IModuleViewPresenter<ICardMachineView>
    {

    }

    public class CardMachinePresenter : ModuleViewPresenter<ICardMachineView>, ICardMachinePresenter
    {
        public CardMachinePresenter(ICardMachineView view) : base(view)
        {
        }
    }
}
