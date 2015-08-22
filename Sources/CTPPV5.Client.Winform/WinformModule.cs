using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using CTPPV5.Client.Winform.Presenters;
using CTPPV5.Client.Winform.Views;
using CTPPV5.Infrastructure.Module;
using CTPPV5.Client.Winform.Api;
using CTPPV5.Client.Winform.Views.Modules;
using WeifenLuo.WinFormsUI.Docking;

namespace CTPPV5.Client.Winform
{
    public class WinformModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new LogModule());
            builder.Register((c, p) => new LoginPresenter(p.Named<ILoginView>("view")))
                .As(typeof(ILoginPresenter))
                .AsSelf();
            builder.Register((c, p) => new ModuleContainerPresenter(p.Named<IModuleContainerView>("view")))
                .As(typeof(IModuleContainerPresenter))
                .AsSelf();
            builder.Register((c, p) => new SchoolNavPresenter(p.Named<ISchoolNavView>("view")))
                .As(typeof(ISchoolNavPresenter))
                .AsSelf();
            builder.Register((c, p) => new CardMachinePresenter(p.Named<ICardMachineView>("view")))
                .As(typeof(ICardMachinePresenter))
                .AsSelf();
            builder.Register((c, p) => new GradeClassPresenter(p.Named<IGradeClassView>("view")))
                .As(typeof(IGradeClassPresenter))
                .AsSelf();
            builder.Register((c, p) => new PermissionMgmPresenter(p.Named<IPermissionMgmView>("view")))
                .As(typeof(IPermissionMgmPresenter))
                .AsSelf();
            builder.Register((c, p) => new SchoolInfoPresenter(p.Named<ISchoolInfoView>("view")))
                .As(typeof(ISchoolInfoPresenter))
                .AsSelf();
            builder.Register((c, p) => new SchoolMgmPresenter(p.Named<ISchoolMgmView>("view")))
                .As(typeof(ISchoolMgmPresenter))
                .AsSelf();
            builder.Register((c, p) => new StudentMgmPresenter(p.Named<IStudentMgmView>("view")))
                .As(typeof(IStudentMgmPresenter))
                .AsSelf();
            builder.Register((c, p) => new TeacherMgmPresenter(p.Named<ITeacherMgmView>("view")))
                .As(typeof(ITeacherMgmPresenter))
                .AsSelf();
            builder.Register((c, p) => new UpdatePwdPresenter(p.Named<IUpdatePwdView>("view")))
                .As(typeof(IUpdatePwdPresenter))
                .AsSelf();
            builder.Register((c, p) => new UserMgmPresenter(p.Named<IUserMgmView>("view")))
                .As(typeof(IUserMgmPresenter))
                .AsSelf();

            builder.Register(c => new RemoteApiImpl()).As<IRemoteApi>().SingleInstance();
            builder.Register((c, p) => new frmCardMachine(p.Named<DockPanel>("parent"))).Keyed<IModule>(ModuleType.CardMachine).ExternallyOwned();
            builder.Register((c, p) => new frmGradeClass(p.Named<DockPanel>("parent"))).Keyed<IModule>(ModuleType.GradeClass).ExternallyOwned();
            builder.Register((c, p) => new frmPermissionMgm(p.Named<DockPanel>("parent"))).Keyed<IModule>(ModuleType.PermissionMgm).ExternallyOwned();
            builder.Register((c, p) => new frmSchoolInfo(p.Named<DockPanel>("parent"))).Keyed<IModule>(ModuleType.SchoolInfo).ExternallyOwned();
            builder.Register((c, p) => new frmSchoolMgm(p.Named<DockPanel>("parent"))).Keyed<IModule>(ModuleType.SchoolMgm).ExternallyOwned();
            builder.Register((c, p) => new frmSchoolNav(p.Named<DockPanel>("parent"))).Keyed<IModule>(ModuleType.SchoolNav).ExternallyOwned();
            builder.Register((c, p) => new frmStudentMgm(p.Named<DockPanel>("parent"))).Keyed<IModule>(ModuleType.StudentMgm).ExternallyOwned();
            builder.Register((c, p) => new frmTeacherMgm(p.Named<DockPanel>("parent"))).Keyed<IModule>(ModuleType.TeacherMgm).ExternallyOwned();
            builder.Register((c, p) => new frmUpdatePwd(p.Named<DockPanel>("parent"))).Keyed<IModule>(ModuleType.UpdatePwd).ExternallyOwned();
            builder.Register((c, p) => new frmUserMgm(p.Named<DockPanel>("parent"))).Keyed<IModule>(ModuleType.UserMgm).ExternallyOwned();
        }
    }
}
