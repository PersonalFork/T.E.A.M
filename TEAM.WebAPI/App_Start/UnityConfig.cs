using System.Web.Http;

using Unity;
using Unity.WebApi;

namespace TEAM.WebAPI
{
    public static partial class UnityConfig
    {
        public static IUnityContainer container;
        public static void RegisterComponents(HttpConfiguration config)
        {
            container = new UnityContainer();
            RegisterTypes(container);

            config.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}