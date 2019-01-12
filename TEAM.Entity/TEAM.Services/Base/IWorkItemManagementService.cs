using System.Collections.Generic;

using TEAM.Business.Dto;

namespace TEAM.Business.Base
{
    public interface IWorkItemManagementService
    {
        WorkItemDto GetWorkItemById(int taskId, int serverId, int employeeId);

        List<WorkItemDto> GetCurrentUserIncompleteItems(int serverId, int employeeId);
    }
}
