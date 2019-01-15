using TEAM.Business.Dto;

namespace TEAM.Business.Base
{
    public interface ILoginService
    {
        bool Logout();
        UserSessionDto Login(string userId, string password);
        UserSessionDto GetUserSessionBySessionId(string sessionId);
    }
}
