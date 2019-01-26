
using NLog;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using TEAM.Business.Base;
using TEAM.Business.Dto;
using TEAM.Business.Extensions;
using TEAM.DAL.Repositories;
using TEAM.Entity;

namespace TEAM.Business
{
    public class WorkItemManagementService : IWorkItemManagementService
    {
        #region Private Variable Declarations.

        private readonly ITeamWorkItemService _teamServerManagementService;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor.

        public WorkItemManagementService()
        {
            _teamServerManagementService = new TfsTeamWorkItemService();
        }

        #endregion

        #region IWorkItemManagementService Implementation.

        public List<WorkItemDto> GetWorkItemByTaskId(int taskId, int weekId)
        {
            using (WorkItemRepository repository = new WorkItemRepository())
            {
                return repository
                    .Filter(x => x.TaskId == taskId && x.WeekId == weekId)
                    .Select(x => x.ToDto(x.Id)).ToList();
            }
        }

        public WorkItemDto GetWorkItemByTaskId(int taskId, int weekId, int serverId)
        {
            WorkItemDto workItem = null;
            using (WorkItemRepository workItemRepository = new WorkItemRepository())
            {
                UserWorkItem entity = workItemRepository.
                    Find(x => x.TaskId == taskId && x.WeekId == weekId && x.ServerId == x.ServerId);
                if (entity != null)
                {
                    return entity.ToDto(entity.Id);
                }
            }
            return workItem;
        }

        public List<WorkItemDto> GetAllWorkItemsByDateRange(DateTime fromDate, DateTime endDate)
        {
            List<WorkItemDto> workItemDtos = new List<WorkItemDto>();

            return workItemDtos;
        }

        public bool DeleteWorkItem(WorkItemDto dto, string userId)
        {
            return false;
        }

        public bool AddWorkItem(WorkItemDto dto, string userId)
        {
            return false;
        }
    
        public bool UpdateWorkItem(WorkItemDto dto, string userId)
        {
            return false;
        }

        public int MoveWorkItemToNext(WorkItemDto workItemDto)
        {
            if (workItemDto == null)
            {
                throw new ArgumentNullException(nameof(workItemDto), "WorkItem cannot be null");
            }

            int workItemWeekId = workItemDto.WeekId;
            int nextWeekId = -1;
            using (WeekInfoRepository weekInfoRepository = new WeekInfoRepository())
            {
                WeekInfo weekInfo = weekInfoRepository.Find(x => x.Id == workItemWeekId);
                if (weekInfo == null)
                {
                    throw new Exception("Week not found.");
                }

                DateTime startDate = weekInfo.StartDate;
                DateTime nextDate = startDate.AddDays(7);
                weekInfo = weekInfoRepository
                    .Find(x => DbFunctions.TruncateTime(x.StartDate) == nextDate);
                if (weekInfo == null)
                {
                    throw new Exception("Next Week not found.");
                }
                nextWeekId = weekInfo.Id;
            }

            using (WorkItemRepository workItemRepository = new WorkItemRepository())
            {
                UserWorkItem workItem = workItemRepository.Find(x => x.Id == workItemDto.Id);
                if (workItem == null)
                {
                    throw new Exception("Work Item not found");
                }

                // Validate if work item already exists in next week.
                UserWorkItem nextWeekWorkItem = workItemRepository
                    .Find(x => x.TaskId == workItemDto.TaskId
                    && x.WeekId == nextWeekId && x.ServerId == workItemDto.ServerId);
                if (nextWeekWorkItem != null)
                {
                    workItemRepository.Delete(nextWeekWorkItem);
                }

                workItem.WeekId = nextWeekId;
                workItem.State = WorkItemState.New;
                int nextWorkItemId = workItemRepository.Insert(workItem);

                UserWorkItem currentWorkItem = workItemRepository.Find(x => x.Id == workItemDto.Id);
                //// update the current work item.
                currentWorkItem.State = WorkItemState.Moved;
                workItemRepository.Update(currentWorkItem);

                return nextWorkItemId;
            }
        }

        public bool UpdateWorkItem(WorkItemDto dto)
        {
            throw new NotImplementedException();
        }

        public bool DeleteWorkItem(WorkItemDto dto)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}