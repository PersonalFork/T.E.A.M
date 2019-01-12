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
            string url = "http://10.4.0.198:8080/tfs/LegacyCollection";
            TeamServerManagementService service = new TeamServerManagementService();
            bool success = service.Authenticate(url, "jchakraborty", "jchakraborty", "ids");
            Assert.IsTrue(success);

        }

        [TestMethod]
        [ExpectedException(typeof(ServiceUnavailableException))]
        public void InvalidUrlLoginCheck()
        {
            string url = "http://10.4.0.190:8080/tfs/LegacyCollection";
            TeamServerManagementService service = new TeamServerManagementService();
            service.Authenticate(url, "jchakraborty", "jchakraborty", "ids");
        }

        [TestMethod]
        [ExpectedException(typeof(TFSAuthenticationException))]
        public void InvalidDomainLoginCheck()
        {
            string url = "http://10.4.0.198:8080/tfs/LegacyCollection";
            TeamServerManagementService service = new TeamServerManagementService();
            service.Authenticate(url, "jchakraborty", "jchakraborty", "ids1");
            //Assert.ThrowsException<TFSAuthenticationException>(() => { });
        }

        [TestMethod]
        [ExpectedException(typeof(TFSAuthenticationException))]
        public void InvalidLoginCheck()
        {
            string url = "http://10.4.0.198:8080/tfs/LegacyCollection";
            TeamServerManagementService service = new TeamServerManagementService();
            service.Authenticate(url, "jchakraborty", "jchakraborty1", "ids");
        }
        #endregion

        #region TFS Server Management.
    
        [TestMethod]
        public void AddTfsServer()
        {
            TeamServerManagementService service = new TeamServerManagementService();
            int id = service.AddTeamServer("TFS OLD", "http://10.4.0.198:8080/tfs/LegacyCollection", 8080);
            Assert.IsTrue(id != 0);
        }

        [TestMethod]
        public void RemoveTfsServer()
        {
            TeamServerManagementService service = new TeamServerManagementService();
            int code = service.DeleteTeamServer(3);
            Assert.IsTrue(code != 0);
        }

        #endregion
    }
}
