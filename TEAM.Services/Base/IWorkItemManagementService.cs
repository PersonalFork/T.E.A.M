using System.Collections.Generic;

using TEAM.Business.Dto;

namespace TEAM.Business.Base
{
    public interface IWorkItemManagementService
    {
        WorkItemDto GetWorkItemById(int taskId, int serverId, string userId);
        List<WorkItemDto> GetCurrentWeekTasks(string userId);
        List<WorkItemDto> GetUserIncompleteItems(int serverId, string userId);
        bool AddWorkItem(WorkItemDto dto, string userId);
        bool UpdateWorkItem(WorkItemDto dto, string userId);
        bool DeleteWorkItem(WorkItemDto dto, string userId);

    }
}
