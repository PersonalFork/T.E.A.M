using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

using TEAM.Business;

namespace TEAM.Test
{
    [TestClass]
    public class TfsServerTest
    {
        #region Get Work Items.
        [TestMethod]
        public void GetValidItemById()
        {
            WorkItemManagementService service = new WorkItemManagementService();
            Business.Dto.WorkItemDto obj = service.GetWorkItemById(200052, 1, "1111111");
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void GetUserIncompleteItems()
        {
            WorkItemManagementService service = new WorkItemManagementService();
            List<Business.Dto.WorkItemDto> obj = service.GetCurrentUserIncompleteItems(1, "1111111");
            Assert.IsTrue(obj.Count > 0);
        }


        [TestMethod]
        public void GetInvalidItemById()
        {
            WorkItemManagementService service = new WorkItemManagementService();
            Business.Dto.WorkItemDto obj = service.GetWorkItemById(1913745, 5, "1111111");
            Assert.IsNull(obj);
        }
        #endregion

        [TestMethod]
        public void AddTfsServer()
        {
            TeamServerManagementService service = new TeamServerManagementService();
            int id = service.AddTeamServer("TFS OLD", Settings.tfsUrl, 8080);
            Assert.IsTrue(id != 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ReAddTfsServer()
        {
            TeamServerManagementService service = new TeamServerManagementService();
            int id = service.AddTeamServer("TFS OLD", Settings.tfsUrl, 8080);
            Assert.IsTrue(id != 0);
        }

        [TestMethod]
        public void RemoveTfsServer()
        {
            TeamServerManagementService service = new TeamServerManagementService();
            int code = service.DeleteTeamServer(2);
            Assert.IsTrue(code != 0);
        }
    }
}
