using System;
using System.Web.Http;

namespace TEAM.Web.Controllers
{
    [RoutePrefix("api/Products")]
    public class ProductsController : ApiController
    {
        [Route("Test", Name = "Test")]
        [HttpGet]
        public string Test()
        {
            return String.Format("{0} {1} ", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
        }
    }
}
