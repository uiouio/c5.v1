using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CTPPV5.Client.Winform.Views.Modules;
using CTPPV5.Models;
using CTPPV5.Client.Winform.Model;
using WinFormsMvp;
using CTPPV5.Infrastructure;
using Autofac;
using CTPPV5.Client.Winform.Api;
using CTPPV5.Models.Api;
using CTPPV5.Client.Winform.Events;

namespace CTPPV5.Client.Winform.Presenters
{
    public interface ISchoolNavPresenter : IPresenter<ISchoolNavView>
    {
        void OnLoad(object sender, EventArgs e);
        void OnSchoolFocused(object sender, ModelEventArgs e);
    }

    public class SchoolNavPresenter : Presenter<ISchoolNavView>, ISchoolNavPresenter
    {
        public SchoolNavPresenter(ISchoolNavView view)
            : base(view)
        {
            view.Load += OnLoad;
            view.SchoolFocused += OnSchoolFocused;
        }

        public void OnLoad(object sender, EventArgs e)
        {
            View.OnLoad();
            using (var scope = ObjectHost.Host.BeginLifetimeScope())
            {
                var classifiedRS = scope.Resolve<IRemoteApi>().GetSchoolListClassified(new ApiRequest());
                if (classifiedRS.Ok)
                {
                    foreach (var province in classifiedRS.Provinces)
                    {
                        View.OnAddSchoolsGroupByProvince(province);
                    }
                    View.OnAddFinished();
                }
            }
        }

        public void OnSchoolFocused(object sender, ModelEventArgs e)
        {
            var focused = e.GetModel<School>();
            if (focused != null) focused.Bind();
        }
    }
}
