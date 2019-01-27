using System.Web.Http;

using TEAM.WebAPI;

namespace TEAM.Web
{
    public static partial class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Disable host-level authentication.
            // Source : https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters#combining-authentication-filters-with-host-level-authentication
            config.SuppressHostPrincipal();

            UnityConfig.RegisterComponents(config);

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
