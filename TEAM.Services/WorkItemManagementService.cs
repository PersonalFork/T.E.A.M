using Microsoft.TeamFoundation.WorkItemTracking.Client;

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

        public WorkItemManagementService(ITeamWorkItemService teamWorkItemService)
        {
            _teamServerManagementService = teamWorkItemService;
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

        public WorkItemDto FindWorkItemFromServer(int taskid, int serverId, int weekId, string userId)
        {
            WorkItemDto workItemDto = null;
            TeamServer server = null;
            using (TeamServerRepository teamServerRepository = new TeamServerRepository())
            {
                server = teamServerRepository.FindLocal(x => x.Id == serverId);
                if (server == null)
                {
                    _logger.Error("Server not found with id " + serverId);
                    throw new Exception("Server not found.");
                }
            }
            string serverUrl = server.Url;
            UserServerInfo userServer = null;
            using (UserServerInfoRepository userServerInfoRepository = new UserServerInfoRepository())
            {
                userServer = userServerInfoRepository.Find(x => x.TfsId == serverId && x.UserId == userId);
                if (userServer == null)
                {
                    throw new Exception(string.Format("User {0} not registered to server {1}", userId, serverId));
                }
            }
            WorkItem workItem = _teamServerManagementService.GetWorkItemById(taskid, serverUrl, userServer.CredentialHash);
            if (workItem != null)
            {
                workItemDto = workItem.ToEntity(serverId).ToDto(-1);
                workItemDto.WeekId = weekId;
                workItemDto.ServerId = serverId;
                workItemDto.TaskId = taskid;
            }
            return workItemDto;
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

        public bool DeleteWorkItemById(int id, string userId)
        {
            using (WorkItemRepository workItemRepository = new WorkItemRepository())
            {
                UserWorkItem workItem = workItemRepository.Find(x => x.Id == id);
                if (workItem != null)
                {
                    workItemRepository.Delete(workItem);
                    return true;
                }
            }
            return false;
        }

        public int AddWorkItem(WorkItemDto dto, string userId)
        {
            UserWorkItem workItem = null;
            using (WorkItemRepository workItemRepository = new WorkItemRepository())
            {
                workItem = workItemRepository
                    .Find(x => x.TaskId == dto.TaskId &&
                    x.WeekId == dto.WeekId &&
                    x.UserId == userId &&
                    x.ServerId == dto.ServerId);

                if (workItem != null)
                {
                    throw new Exception("Work Item already exists");
                }

                // Add a new work item.
                UserWorkItem userWorkItem = new UserWorkItem
                {
                    WeekId = dto.WeekId,
                    UserId = dto.UserId,
                    TaskId = dto.TaskId,
                    ServerId = dto.ServerId,
                    Title = dto.Title,
                    Description = dto.Description
                };

                return workItemRepository.Insert(userWorkItem);
            }
        }

        public bool DeleteWorkItem(WorkItemDto dto, string userId)
        {
            using (WorkItemRepository workItemRepository = new WorkItemRepository())
            {
                UserWorkItem workItem = workItemRepository.Find(x => x.Id == dto.Id);
                if (workItem != null)
                {
                    workItemRepository.Delete(workItem);
                    return true;
                }
            }
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
                    throw new Exception("Previous Week not found.");
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

        public int MoveWorkItemToPrevious(WorkItemDto workItemDto)
        {
            if (workItemDto == null)
            {
                throw new ArgumentNullException(nameof(workItemDto), "WorkItem cannot be null");
            }

            int workItemWeekId = workItemDto.WeekId;
            int previousWeekId = -1;
            using (WeekInfoRepository weekInfoRepository = new WeekInfoRepository())
            {
                WeekInfo weekInfo = weekInfoRepository.Find(x => x.Id == workItemWeekId);
                if (weekInfo == null)
                {
                    throw new Exception("Week not found.");
                }

                DateTime startDate = weekInfo.StartDate;
                DateTime nextDate = startDate.AddDays(-7);
                weekInfo = weekInfoRepository
                    .Find(x => DbFunctions.TruncateTime(x.StartDate) == nextDate);
                if (weekInfo == null)
                {
                    throw new Exception("Next Week not found.");
                }
                previousWeekId = weekInfo.Id;
            }

            using (WorkItemRepository workItemRepository = new WorkItemRepository())
            {
                UserWorkItem workItem = workItemRepository.Find(x => x.Id == workItemDto.Id);
                if (workItem == null)
                {
                    throw new Exception("Work Item not found");
                }

                // Validate if work item already exists in previous week.
                UserWorkItem previousWeekWorkItem = workItemRepository
                    .Find(x => x.TaskId == workItemDto.TaskId &&
                    x.WeekId == previousWeekId &&
                    x.ServerId == workItemDto.ServerId);
                if (previousWeekWorkItem != null)
                {
                    workItemRepository.Delete(previousWeekWorkItem);
                }

                workItem.WeekId = previousWeekId;
                workItem.State = WorkItemState.New;
                int nextWorkItemId = workItemRepository.Insert(workItem);

                UserWorkItem currentWorkItem = workItemRepository.Find(x => x.Id == workItemDto.Id);
                // update the current work item.
                currentWorkItem.State = WorkItemState.Moved;
                workItemRepository.Update(currentWorkItem);

                return nextWorkItemId;
            }
        }

        public int CopyWorkItemToNext(WorkItemDto workItemDto)
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
                    .Find(x => x.TaskId == workItemDto.TaskId &&
                    x.WeekId == nextWeekId &&
                    x.ServerId == workItemDto.ServerId);
                if (nextWeekWorkItem != null)
                {
                    workItemRepository.Delete(nextWeekWorkItem);
                }

                // Clone the work item. 
                workItem.WeekId = nextWeekId;
                workItem.State = WorkItemState.New;
                int nextWorkItemId = workItemRepository.Insert(workItem);

                return nextWorkItemId;
            }
        }

        public int CopyWorkItemToPrevious(WorkItemDto workItemDto)
        {
            if (workItemDto == null)
            {
                throw new ArgumentNullException(nameof(workItemDto), "WorkItem cannot be null");
            }

            int workItemWeekId = workItemDto.WeekId;
            int previousWeekId = -1;
            using (WeekInfoRepository weekInfoRepository = new WeekInfoRepository())
            {
                WeekInfo weekInfo = weekInfoRepository.Find(x => x.Id == workItemWeekId);
                if (weekInfo == null)
                {
                    throw new Exception("Week not found.");
                }

                DateTime startDate = weekInfo.StartDate;
                DateTime nextDate = startDate.AddDays(-7);
                weekInfo = weekInfoRepository
                    .Find(x => DbFunctions.TruncateTime(x.StartDate) == nextDate);
                if (weekInfo == null)
                {
                    throw new Exception("Previous Week not found.");
                }
                previousWeekId = weekInfo.Id;
            }

            using (WorkItemRepository workItemRepository = new WorkItemRepository())
            {
                UserWorkItem workItem = workItemRepository.Find(x => x.Id == workItemDto.Id);
                if (workItem == null)
                {
                    throw new Exception("Work Item not found");
                }

                // Validate if work item already exists in next week.
                UserWorkItem previousWeekWorkItem = workItemRepository
                    .Find(x => x.TaskId == workItemDto.TaskId &&
                    x.WeekId == previousWeekId &&
                    x.ServerId == workItemDto.ServerId);
                if (previousWeekWorkItem != null)
                {
                    workItemRepository.Delete(previousWeekWorkItem);
                }

                // Clone the work item. 
                workItem.WeekId = previousWeekId;
                workItem.State = WorkItemState.New;
                int previousWorkItemId = workItemRepository.Insert(workItem);

                return previousWorkItemId;
            }
        }

        #endregion
    }
}