using TEAM.Business;
using TEAM.Business.Base;

using Unity;

namespace TEAM.WebAPI
{
    public static partial class UnityConfig
    {
        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ILoginService, LoginService>();
            container.RegisterType<ITeamWorkItemService, TfsTeamWorkItemService>();
            container.RegisterType<IWorkItemSyncService, WorkItemSyncService>();
            container.RegisterType<IWorkItemManagementService, WorkItemManagementService>();
            container.RegisterType<IUserManagementService, UserManagementService>();
        }
    }
}