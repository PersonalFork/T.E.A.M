using System;
using System.Collections.Generic;

using TEAM.Business.Dto;

namespace TEAM.Business.Base
{
    public interface IWorkItemManagementService
    {
        List<WorkItemDto> GetWorkItemByTaskId(int taskId, int weekId);
        WorkItemDto GetWorkItemByTaskId(int taskId, int weekId, int serverId);
        List<WorkItemDto> GetAllWorkItemsByDateRange(DateTime fromDate, DateTime endDate);
        int MoveWorkItemToNext(WorkItemDto dto);
        bool UpdateWorkItem(WorkItemDto dto);
        bool DeleteWorkItem(WorkItemDto dto);
        bool AddWorkItem(WorkItemDto dto, string userId);
    }
}
