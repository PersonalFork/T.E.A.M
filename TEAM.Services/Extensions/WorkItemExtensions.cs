using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TEAM.Business.Dto;

namespace TEAM.Business.Extensions
{
    public static class WorkItemExtensions
    {
        public static WorkItemDto ToDto(this WorkItem workItem, int serverId)
        {
            WorkItemDto dto = new WorkItemDto
            {
                Id = workItem.Id,
                ServerId = serverId,
                TaskId = workItem.Id,
                Description = workItem.Description,
                Title = workItem.Title,
                Project = workItem.State
            };
            return dto;
        }
    }
}
