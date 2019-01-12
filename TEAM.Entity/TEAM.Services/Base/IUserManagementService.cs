using TEAM.Business.Dto;

namespace TEAM.Business.Base
{
    public interface IUserManagementService
    {
        int RegisterUser(UserRegistrationDto userRegistrationDto);
        int RegisterServer(int serverId, int userId, string serverUserId, string serverPassword, string serverDomain);
    }
}