using System.Web.Http;

using TEAM.WebAPI;

using Unity;

namespace TEAM.Web
{
    public static partial class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Disable host-level authentication.
            // Source : https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters#combining-authentication-filters-with-host-level-authentication
            config.SuppressHostPrincipal();

            // Web API configuration and services
            UnityContainer container = new UnityContainer();
            RegisterTypes(container);
            config.DependencyResolver = new UnityResolver(container);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
