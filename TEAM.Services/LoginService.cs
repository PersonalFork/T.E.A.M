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
                return new UserSessionDto()
                {
                    SessionId = Guid.NewGuid().ToString(),
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
