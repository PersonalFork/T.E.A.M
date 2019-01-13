using Newtonsoft.Json;

using NLog;

using System;
using System.Net;

using TEAM.Business.Base;
using TEAM.Business.Dto;
using TEAM.Common;
using TEAM.DAL.Repositories;
using TEAM.Entity;

namespace TEAM.Business
{
    public class UserManagementService : IUserManagementService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly TeamServerManagementService _authenticationService;
        public UserManagementService()
        {
            _authenticationService = new TeamServerManagementService();
        }

        public int RegisterUser(UserRegistrationDto userRegistrationDto)
        {
            UserLoginDto userLogin = userRegistrationDto.LoginInfo;
            UserInfoDto userInfo = userRegistrationDto.UserInfo;
            int userId;

            try
            {
                if (userLogin == null)
                {
                    throw new ArgumentNullException("User Login Information cannot be null");
                }

                if (userInfo == null)
                {
                    throw new ArgumentNullException("User Information cannot be null");
                }

                using (UserLoginRepository userLoginRepository = new UserLoginRepository())
                {
                    UserLogin userLoginEntity = userLoginRepository.Find(x => string.Equals(x.UserId, userLogin.UserId));
                    if (userLoginEntity != null)
                    {
                        throw new Exception("User with same user id already exists.");
                    }
                    else
                    {
                        userLoginEntity = new UserLogin()
                        {
                            UserId = userLogin.UserId,
                            Password = userLogin.Password.Encrypt(),
                            IsActive = true,
                            IsLocked = false,
                            RetryCount = 0
                        };
                        userId = userLoginRepository.Insert(userLoginEntity);
                    }
                }
                if (userId != 0)
                {
                    using (UserInfoRepository userInfoRepository = new UserInfoRepository())
                    {
                        UserInfo userInfoEntity = new UserInfo()
                        {
                            UserId = userInfo.UserId,
                            EMail = userInfo.Email,
                            FirstName = userInfo.FirstName,
                            LastName = userInfo.LastName,
                            Gender = userInfo.Gender
                        };
                        return userInfoRepository.Insert(userInfoEntity);
                    }
                }
                else
                {
                    throw new Exception("Failed to register user.");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public int RegisterServer(int serverId, string userId, string serverUserId, string serverPassword, string serverDomain)
        {
            TeamServer teamServerEntity;
            try
            {
                using (TeamServerRepository teamServerRepository = new TeamServerRepository())
                {
                    teamServerEntity = teamServerRepository.GetById(serverId);
                    if (teamServerEntity == null)
                    {
                        throw new Exception("Invalid server id");
                    }
                }
                using (UserInfoRepository userInfoRepository = new UserInfoRepository())
                {
                    UserInfo userInfoEntity = userInfoRepository.Find(x => x.UserId == userId);
                    if (userInfoEntity == null)
                    {
                        throw new Exception("Invalid user id");
                    }
                }
                using (UserServerInfoRepository userServerInfoRepository = new UserServerInfoRepository())
                {
                    UserServerInfo userServerInfoEntity = userServerInfoRepository.Find(
                        x => x.UserId.ToUpper() == userId.ToUpper() && x.TfsId == serverId);
                    if (userServerInfoEntity != null)
                    {
                        throw new Exception(string.Format("Server {0} is already registered to the user {1} .", serverId, userId));
                    }

                    // Dependency Injection of Team Service.
                    NetworkCredential credential = new NetworkCredential(serverUserId, serverPassword, serverDomain);
                    string hash = JsonConvert.SerializeObject(credential).Encrypt();
                    _authenticationService.Authenticate(serverId, hash);

                    userServerInfoEntity = new UserServerInfo()
                    {
                        UserId = userId,
                        TfsId = serverId,
                        TfsUserId = serverUserId,
                        CredentialHash = hash
                    };
                    return userServerInfoRepository.Insert(userServerInfoEntity);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}