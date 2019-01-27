using log4net;

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using TEAM.Business.Base;
using TEAM.Business.Dto;
using TEAM.WebAPI.Common;

namespace TEAM.WebAPI.Controllers
{
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        public static readonly ILog _loggger = LogManager.GetLogger(typeof(LoginController));
        private ILoginService _loginService;

        //[Dependency]
        //private ILoginService LoginService { get; set; }

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        [Route("doLogin")]
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

                // Attach a session cookie to response.
                CookieHeaderValue cookie = new CookieHeaderValue(Constants.SESSION_KEY, userSession.SessionId)
                {
                    Expires = DateTimeOffset.Now.AddDays(1),
                    Domain = Request.RequestUri.Host,
                    Path = "/"
                };
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, userSession);
                response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                return response;
            }
            catch (Exception ex)
            {
                _loggger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("doLogout")]
        public HttpResponseMessage Logout()
        {
            UserSessionDto userSession = Request.GetUserSession();
            if (userSession == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Session not found.");
            }
            try
            {
                bool isLogoutSuccess = _loginService.Logout(userSession.SessionId);
                if (isLogoutSuccess)
                {
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, "User has logged off successfully.");
                    CookieHeaderValue cookie = new CookieHeaderValue(Constants.SESSION_KEY, userSession.SessionId)
                    {
                        Expires = DateTimeOffset.Now.AddDays(-1),
                        Domain = Request.RequestUri.Host,
                        Path = "/"
                    };
                    response.Headers.AddCookies(new CookieHeaderValue[] { cookie });
                    return response;
                }
                else
                {
                    HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not logoff.");
                    return response;
                }
            }
            catch (Exception)
            {
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Could not login.");
                return response;
            }
        }
    }
}