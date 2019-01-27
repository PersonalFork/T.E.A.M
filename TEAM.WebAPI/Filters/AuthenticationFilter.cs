using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

using TEAM.Business.Dto;
using TEAM.WebAPI.Common;

namespace TEAM.WebAPI.Filters
{
    /// <summary>
    /// Authentication Filter for the project.
    /// Source : https://docs.microsoft.com/en-us/aspnet/web-api/overview/security/authentication-filters
    /// </summary>
    public class AuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple { get; private set; }

        public override bool Match(object obj)
        {
            return base.Match(obj);
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            if (request != null)
            {
                UserSessionDto session = request.GetUserSession();
                if (session == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("User Session not found", request);
                    return;
                }
                else
                {
                    context.Principal = new ClaimsPrincipal();
                    return;
                }
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            //AuthenticationHeaderValue challenge = new AuthenticationHeaderValue("Basic");
            //context.Result = new AddChallengeOnUnauthorizedResult(challenge, context.Result);
            return Task.FromResult(0);
        }
    }
}