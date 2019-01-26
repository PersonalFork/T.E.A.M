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
            TfsTeamWorkItemService service = new TfsTeamWorkItemService();
            bool success = service.Authenticate(Settings.tfsUrl, "jchakraborty", "jchakraborty", "ids");
            Assert.IsTrue(success);
        }

        [TestMethod]
        [ExpectedException(typeof(ServiceUnavailableException))]
        public void InvalidUrlLoginCheck()
        {
            TfsTeamWorkItemService service = new TfsTeamWorkItemService();
            service.Authenticate(Settings.tfsUrl + "wrongUrl", "jchakraborty", "jchakraborty", "ids");
        }

        [TestMethod]
        [ExpectedException(typeof(TFSAuthenticationException))]
        public void InvalidDomainLoginCheck()
        {
            TfsTeamWorkItemService service = new TfsTeamWorkItemService();
            service.Authenticate(Settings.tfsUrl, "jchakraborty", "jchakraborty", "ids1");
        }

        [TestMethod]
        [ExpectedException(typeof(TFSAuthenticationException))]
        public void InvalidLoginCheck()
        {
            TfsTeamWorkItemService service = new TfsTeamWorkItemService();
            service.Authenticate(Settings.tfsUrl, "jchakraborty", "jchakraborty1", "ids");
        }

        #endregion
    }
}
