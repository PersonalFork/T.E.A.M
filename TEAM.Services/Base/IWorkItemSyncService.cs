using System;
using System.Collections.Generic;

using TEAM.Business.Dto;


namespace TEAM.Business.Base
{
    public interface IWorkItemSyncService
    {
        WorkItemDto GetWorkItemById(int taskId, int serverId, string userId);
        List<WorkItemDto> GetUserIncompleteSyncedTasks(int serverId, string userId);
        List<WorkItemDto> GetUserCurrentWeekSyncedTasks(string userId, bool includeIncompleteItems = true);
        List<WorkItemDto> GetUserSyncedTasksByDateRange(DateTime fromDate, DateTime endDate, string userId, bool includeIncompleteItems = false);
    }
}
