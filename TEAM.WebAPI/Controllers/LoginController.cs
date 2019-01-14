using log4net;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TEAM.Business;
using TEAM.Business.Base;
using TEAM.Business.Dto;

using Unity.Attributes;

namespace TEAM.WebAPI.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        public static readonly ILog _loggger = LogManager.GetLogger(typeof(LoginController));
        private ILoginService _loginService;

        //[Dependency]
        //private ILoginService LoginService { get; set; }

        public LoginController()
        {
            _loginService = new LoginService();
        }

        [HttpPost]
        [Route("Test")]
        public HttpResponseMessage Login([FromBody]UserLoginDto userLoginDto)
        {
            if (userLoginDto == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User login cannot be empty.");
            }
            if (string.IsNullOrEmpty(userLoginDto.UserId))
            {
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User Id cannot be empty");
                return response;
            }
            if (string.IsNullOrEmpty(userLoginDto.Password))
            {
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Password cannot be empty");
                return response;
            }

            try
            {
                UserSessionDto userSession = _loginService.Login(userLoginDto.UserId, userLoginDto.Password);
                return Request.CreateResponse(HttpStatusCode.OK, userSession);
            }
            catch (Exception ex)
            {
                _loggger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}