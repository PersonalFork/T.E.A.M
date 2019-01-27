using System.Web;
using System.Web.Http;

using TEAM.WebAPI;

namespace TEAM.Web
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {    
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
