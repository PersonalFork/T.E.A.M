using Microsoft.VisualStudio.TestTools.UnitTesting;
using TEAM.Business;
using TEAM.Business.Base;
using TEAM.Business.Dto;

namespace TEAM.Test
{
    [TestClass]
    public class WorkItemTest
    {
        private readonly ITeamWorkItemService _teamWorkItemService;
        private readonly IWorkItemManagementService _workItemManagementService;
        private readonly IWorkItemSyncService _userWorkItemQueryService;
        public WorkItemTest()
        {
            _teamWorkItemService = new TfsTeamWorkItemService();
            _workItemManagementService = new WorkItemManagementService();
            _userWorkItemQueryService = new WorkItemSyncService(_teamWorkItemService);
        }

        [TestMethod]
        public void MoveToNext()
        {
            WorkItemDto workItem = _workItemManagementService.GetWorkItemByTaskId(212204, 3, 1);
            int id = _workItemManagementService.MoveWorkItemToNext(workItem);
            Assert.IsNotNull(workItem);
        }
    }
}
