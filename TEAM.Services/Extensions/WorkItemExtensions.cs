using Microsoft.TeamFoundation.WorkItemTracking.Client;

using TEAM.Business.Dto;
using TEAM.Entity;

namespace TEAM.Business.Extensions
{
    public static class WorkItemExtensions
    {
        public static UserWorkItem ToEntity(this WorkItem tfsWorkItem, int serverId)
        {
            return new UserWorkItem()
            {
                TaskId = tfsWorkItem.Id,
                Title = tfsWorkItem.Title,
                Description = tfsWorkItem.Description,
                Sprint = tfsWorkItem.IterationPath,
                Status = tfsWorkItem.State,
                Project = tfsWorkItem.AreaPath,
                ServerId = serverId
            };
        }

        public static WorkItemDto ToDto(this UserWorkItem userWorkItem, int id)
        {
            WorkItemDto dto = new WorkItemDto
            {
                Id = id,
                TaskId = userWorkItem.TaskId,
                UserId = userWorkItem.UserId,
                WeekId = userWorkItem.WeekId,
                Title = userWorkItem.Title,
                Description = userWorkItem.Description,
                Sprint = userWorkItem.Sprint,
                Project = userWorkItem.Project,
                TotalHours = userWorkItem.TotalHours,
                WeekHours = userWorkItem.WeekHours,
                Status = userWorkItem.Status,
                ServerId = userWorkItem.ServerId,
                Comments = userWorkItem.Comments,
                EstimatedHours = userWorkItem.EstimatedHours,
                AssignedTo = userWorkItem.AssignedTo
            };
            if (userWorkItem.StartDate != null)
            {
                dto.StartDate = userWorkItem.StartDate.Value.ToString("dd-MM-yyyy");
            }

            if (userWorkItem.EndDate != null)
            {
                dto.EndDate = userWorkItem.EndDate.Value.ToString("dd-MM-yyyy");
            }

            if (userWorkItem.ETA != null)
            {
                dto.ETA = userWorkItem.ETA.Value.ToString("dd-MM-yyyy");
            }
            return dto;
        }
    }
}
