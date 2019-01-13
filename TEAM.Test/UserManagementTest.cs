using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using TEAM.Business;
using TEAM.Business.Dto;
using TEAM.Common.Exceptions;

namespace TEAM.Test
{
    [TestClass]
    public class UserManagementTest
    {
        [TestMethod]
        public void RegisterUser()
        {
            UserLoginDto userLoginDto = new UserLoginDto()
            {
                UserId = "1111111",
                Password = "Nec@12345"
            };
            UserInfoDto userInfoDto = new UserInfoDto()
            {
                UserId = "1111111",
                Email = "jashobanta.c@nec.com",
                FirstName = "Jashobanta",
                LastName = "Chakraborty",
                Gender = "Male"
            };
            UserRegistrationDto userRegistrationDto = new UserRegistrationDto
            {
                LoginInfo = userLoginDto,
                UserInfo = userInfoDto
            };
            UserManagementService userManagementService = new UserManagementService();
            userManagementService.RegisterUser(userRegistrationDto);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReRegisterUser()
        {
            RegisterUser();
        }

        [TestMethod]
        public void AddServer()
        {
            UserManagementService service = new UserManagementService();
            int ret = service.RegisterServer(1, "1111111", Settings.userId, Settings.password, Settings.domain);
            Assert.IsFalse(ret == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(TFSAuthenticationException))]
        public void AddServerWithWrongTFSCredentials()
        {
            UserManagementService service = new UserManagementService();
            service.RegisterServer(4, "1111111", Settings.userId, Settings.password, Settings.domain);
        }

    }
}
