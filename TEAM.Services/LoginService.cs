using System;

using TEAM.Business.Base;
using TEAM.Business.Dto;
using TEAM.Common;
using TEAM.DAL.Repositories;
using TEAM.Entity;

namespace TEAM.Business
{
    public class LoginService : ILoginService
    {
        public UserSessionDto GetUserSessionBySessionId(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                return null;
            }
            UserSession userSession = null;
            using (UserSessionRepository userSessionRepository = new UserSessionRepository())
            {
                userSession = userSessionRepository.Find(x => x.SessionId == sessionId);
            }
            if (userSession == null)
            {
                return null;
            }
            else
            {
                UserInfo userInfo = new UserInfo();
                using (UserInfoRepository userInfoRepository = new UserInfoRepository())
                {
                    userInfo = userInfoRepository.Find(x => x.UserId == userSession.UserId);
                }
                if (userInfo != null)
                {
                    return new UserSessionDto()
                    {
                        SessionId = sessionId,
                        User = new UserInfoDto()
                        {
                            FirstName = userInfo.FirstName,
                            LastName = userInfo.LastName,
                            Email = userInfo.EMail,
                            UserId = userInfo.UserId,
                            Gender = userInfo.Gender
                        }
                    };
                }
            }
            return null;
        }

        public UserSessionDto Login(string userId, string password)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User Id cannot be null", nameof(userId));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null", nameof(password));
            }

            UserLogin userLoginEntity = null;
            using (UserLoginRepository userLoginRepository = new UserLoginRepository())
            {
                string securePassword = password.Encrypt();
                userLoginEntity = userLoginRepository.Find(x => x.UserId == userId && x.Password == securePassword);
                if (userLoginEntity == null)
                {
                    throw new ApplicationException("Invalid UserId/Password");
                }
            }
            using (UserInfoRepository repository = new UserInfoRepository())
            {
                UserInfo userInfo = repository.Find(x => x.UserId == userId);
                if (userInfo == null)
                {
                    throw new ApplicationException("User Info not found.");
                }
                string sessionId = Guid.NewGuid().ToString();
                using (UserSessionRepository userSessionRepository = new UserSessionRepository())
                {
                    UserSession userSession = new UserSession
                    {
                        UserId = userId,
                        SessionId = sessionId,
                        ValidFrom = DateTime.Now,
                        ExpiresOn = DateTime.Now.AddDays(1)
                    };
                    userSessionRepository.Insert(userSession);
                }

                return new UserSessionDto()
                {
                    SessionId = sessionId,
                    User = new UserInfoDto()
                    {
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        Email = userInfo.EMail,
                        UserId = userInfo.UserId,
                        Gender = userInfo.Gender
                    }
                };
            }
        }

        public bool Logout()
        {
            return true;
        }
    }
}
