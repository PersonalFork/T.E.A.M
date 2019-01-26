using TEAM.Business;
using TEAM.Business.Base;

using Unity;

namespace TEAM.Web
{
    public static partial class WebApiConfig
    {
        private static void RegisterTypes(UnityContainer container)
        {
            container.RegisterType<ILoginService, LoginService>();
            container.RegisterType<ITeamWorkItemService, TfsTeamWorkItemService>();
        }
    }
}