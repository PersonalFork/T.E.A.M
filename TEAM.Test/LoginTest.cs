using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

using TEAM.Business;
using TEAM.Business.Dto;
using TEAM.Common;

namespace TEAM.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class LoginTest
    {
        #region Login.

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoginBlankUserId()
        {
            string userId = "";
            string password = "password";
            LoginService loginService = new LoginService();
            UserSessionDto userSession = loginService.Login(userId, password);
            Assert.IsNotNull(userSession);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoginBlankPassword()
        {
            string userId = "1111111";
            string password = "";
            LoginService loginService = new LoginService();
            UserSessionDto userSession = loginService.Login(userId, password);
            Assert.IsNotNull(userSession);
        }

        [TestMethod]
        public void ValidLogin()
        {
            string userId = "1111111";
            string password = "Nec@12345";

            LoginService loginService = new LoginService();
            UserSessionDto userSession = loginService.Login(userId, password);
            Assert.IsNotNull(userSession);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void InvalidUserId()
        {
            string userId = "1111112";
            string password = "Nec@12345";

            LoginService loginService = new LoginService();
            UserSessionDto userSession = loginService.Login(userId, password);
            Assert.IsNotNull(userSession);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void InvalidPassword()
        {
            string userId = "1111111";
            string password = "Nec";
            LoginService loginService = new LoginService();
            UserSessionDto userSession = loginService.Login(userId, password);
            Assert.IsNotNull(userSession);
        }

        #endregion
    }
}
