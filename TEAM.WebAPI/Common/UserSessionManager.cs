using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

using TEAM.Business;
using TEAM.Business.Base;
using TEAM.Business.Dto;

namespace TEAM.WebAPI.Common
{
    public static class RequestExtensions
    {
        private static readonly ILoginService _loginService = null;
        static RequestExtensions()
        {
            _loginService = new LoginService();
        }

        public static UserSessionDto GetUserSession(this HttpRequestMessage request)
        {
            CookieHeaderValue sessionCookie = request.Headers.GetCookies(Constants.SESSION_KEY).FirstOrDefault();
            if (sessionCookie != null && sessionCookie.Cookies.Count > 0)
            {
                string userId = string.Empty;
                string sessionId = sessionCookie.Cookies[0].Value;
                if (!string.IsNullOrEmpty(sessionId))
                {
                    return _loginService.GetUserSessionBySessionId(sessionId);
                }
            }
            return null;
        }
    }
}