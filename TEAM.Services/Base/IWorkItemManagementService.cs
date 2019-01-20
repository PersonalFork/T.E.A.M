using System.Collections.Generic;

using TEAM.Business.Dto;

namespace TEAM.Business.Base
{
    public interface IWorkItemManagementService
    {
        WorkItemDto GetWorkItemById(int taskId, int serverId, string userId);

        List<WorkItemDto> GetCurrentUserIncompleteItems(int serverId, string userId);

        List<WorkItemDto> Sync(int weekId, List<WorkItemDto> workItems, string userId);
    }
}
