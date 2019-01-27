using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using TEAM.Business;
using TEAM.Business.Base;
using TEAM.Business.Dto;
using TEAM.Common.Exceptions;

namespace TEAM.Test
{
    [TestClass]
    public class UserManagementTest
    {
        private readonly ITeamWorkItemService _teamWorkItemService;

        public UserManagementTest()
        {
            _teamWorkItemService = new TfsTeamWorkItemService();
        }

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
            UserManagementService userManagementService = new UserManagementService(_teamWorkItemService);
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
            UserManagementService service = new UserManagementService(_teamWorkItemService);
            int ret = service.RegisterServer(1, "1111111", Settings.userId, Settings.password, Settings.domain);
            Assert.IsFalse(ret == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(TFSAuthenticationException))]
        public void AddServerWithWrongTFSCredentials()
        {
            UserManagementService service = new UserManagementService(_teamWorkItemService);
            service.RegisterServer(4, "1111111", Settings.userId, Settings.password, Settings.domain);
        }

        [TestMethod]
        public void GetValidUserServerList()
        {
            UserManagementService service = new UserManagementService(_teamWorkItemService);
            System.Collections.Generic.List<UserServerDto> servers = service.GetUserServerList("1111111");
            Assert.IsTrue(servers.Count > 0);
        }
    }
}
