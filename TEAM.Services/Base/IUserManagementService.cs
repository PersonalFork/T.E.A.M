using System.Collections.Generic;
using TEAM.Business.Dto;

namespace TEAM.Business.Base
{
    public interface IUserManagementService
    {
        int RegisterUser(UserRegistrationDto userRegistrationDto);
        int RegisterServer(int serverId, string userId, string serverUserId, string serverPassword, string serverDomain);
        List<UserServerDto> GetUserServerList(string userId);
    }
}