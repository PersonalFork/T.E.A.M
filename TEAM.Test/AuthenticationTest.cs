using Microsoft.VisualStudio.TestTools.UnitTesting;

using TEAM.Business;
using TEAM.Common.Exceptions;

namespace TEAM.Test
{
    [TestClass]
    public class AuthenticationTest
    {
        #region Authentication.

        [TestMethod]
        public void ValidLoginCheck()
        {
            TeamServerManagementService service = new TeamServerManagementService();
            bool success = service.Authenticate(Settings.tfsUrl, "jchakraborty", "jchakraborty", "ids");
            Assert.IsTrue(success);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceUnavailableException))]
        public void InvalidUrlLoginCheck()
        {
            TeamServerManagementService service = new TeamServerManagementService();
            service.Authenticate(Settings.tfsUrl + "wrongUrl", "jchakraborty", "jchakraborty", "ids");
        }

        [TestMethod]
        [ExpectedException(typeof(TFSAuthenticationException))]
        public void InvalidDomainLoginCheck()
        {
            TeamServerManagementService service = new TeamServerManagementService();
            service.Authenticate(Settings.tfsUrl, "jchakraborty", "jchakraborty", "ids1");
        }

        [TestMethod]
        [ExpectedException(typeof(TFSAuthenticationException))]
        public void InvalidLoginCheck()
        {
            TeamServerManagementService service = new TeamServerManagementService();
            service.Authenticate(Settings.tfsUrl, "jchakraborty", "jchakraborty1", "ids");
        }
        #endregion
    }
}
