using Microsoft.TeamFoundation.WorkItemTracking.Client;
using TEAM.Business.Dto;

namespace TEAM.Business.Extensions
{
    public static class WorkItemExtensions
    {
        public static WorkItemDto ToDto(this WorkItem workItem)
        {
            WorkItemDto dto = new WorkItemDto
            {
                TaskId = workItem.Id,
                Title = workItem.Title,
                Status = workItem.State
            };
            return dto;
        }
    }
}
