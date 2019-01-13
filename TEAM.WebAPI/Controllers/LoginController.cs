using System.Net;
using System.Net.Http;
using System.Web.Http;

using TEAM.Business.Base;
using TEAM.Business.Dto;

using Unity.Attributes;

namespace TEAM.WebAPI.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [Dependency]
        private ITeamServerManagementService TeamServerManagementService { get; set; }

        [HttpPost]
        public HttpResponseMessage Login([FromBody]UserLoginDto userLoginDto)
        {
            if (userLoginDto == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User login cannot be empty.");
            }
            if (string.IsNullOrEmpty(userLoginDto.UserId))
            {
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Employee Id cannot be empty");
                return response;
            }
            if (string.IsNullOrEmpty(userLoginDto.Password))
            {
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Password cannot be empty");
                return response;
            }

            try
            {
                TeamServerManagementService.Authenticate("ab", "ab", "ab", "ab");
            }
            catch
            {

            }
            return null;
        }
    }
}