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
    public class WorkItemSyncService : IWorkItemSyncService
    {
        #region Private Variable Declarations.

        private readonly ITeamWorkItemService _teamServerManagementService;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructor.

        public WorkItemSyncService(ITeamWorkItemService teamWorkItemService)
        {
            _teamServerManagementService = teamWorkItemService;
        }

        #endregion

        #region IUserWorkItemManagementService Implementation.

        public WorkItemDto GetWorkItemById(int taskId, int serverId, int weekId, string userId)
        {
            UserWorkItem userWorkItem = null;
            using (WorkItemRepository repository = new WorkItemRepository())
            {
                userWorkItem = repository.Find(
                    x => x.TaskId == taskId &&
                    x.ServerId == serverId &&
                    x.WeekId == weekId
                    && x.UserId == userId);
            }
            if (userWorkItem == null)
            {
                return null;
            }
            return userWorkItem.ToDto(userWorkItem.Id);
        }

        public WorkItemDto GetWorkItemById(int taskId, int serverId, string userId)
        {
            TeamServer server = null;
            UserServerInfo userServerInfo = null;
            try
            {
                using (TeamServerRepository teamServerRepository = new TeamServerRepository())
                {
                    server = teamServerRepository.GetById(serverId);
                    if (server == null)
                    {
                        throw new Exception(string.Format("Invalid Server Id : {0}", serverId));
                    }
                }
                UserInfo userInfo = null;
                using (UserInfoRepository userInfoRepository = new UserInfoRepository())
                {
                    userInfo = userInfoRepository.Find(x => x.UserId != null && x.UserId.ToUpper() == userId.ToUpper());
                    if (userInfo == null)
                    {
                        throw new Exception(string.Format("User with ID {0} Not Found", userId));
                    }
                }

                using (UserServerInfoRepository userServerInfoRepository = new UserServerInfoRepository())
                {
                    userServerInfo = userServerInfoRepository.FindLocal(
                        x => x.UserId != null
                        && x.UserId.ToUpper() == userId.ToUpper()
                        && x.TfsId == serverId);
                    if (userServerInfo == null)
                    {
                        throw new Exception(string.Format("User : {0} is not registered with server id : {1}", userId, serverId));
                    }
                    string credentialHash = userServerInfo.CredentialHash;
                    string url = server.Url;
                    WorkItem workItem = _teamServerManagementService.GetWorkItemById(taskId, url, credentialHash);
                    if (workItem != null)
                    {
                        return workItem.ToEntity(serverId).ToDto(workItem.Id);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public List<WorkItemDto> GetUserIncompleteSyncedTasks(int serverId, string userId)
        {
            TeamServer server = null;
            List<WorkItemDto> workItemsDtoList = new List<WorkItemDto>();
            UserServerInfo userServerInfo = null;
            try
            {
                using (TeamServerRepository teamServerRepository = new TeamServerRepository())
                {
                    server = teamServerRepository.GetById(serverId);
                    if (server == null)
                    {
                        throw new Exception(string.Format("Invalid Server Id : {0}", serverId));
                    }
                }

                UserInfo userInfo = null;
                using (UserInfoRepository userInfoRepository = new UserInfoRepository())
                {
                    userInfo = userInfoRepository.Find(x => x.UserId == userId);
                    if (userInfo == null)
                    {
                        throw new Exception(string.Format("User with ID {0} Not Found", userId));
                    }
                }

                using (UserServerInfoRepository userServerInfoRepository = new UserServerInfoRepository())
                {
                    userServerInfo = userServerInfoRepository.FindLocal(
                        x => x.UserId == userId
                        && x.TfsId == serverId
                    );
                    if (userServerInfo == null)
                    {
                        throw new Exception(string.Format("User with id : {0} is not registered with server id : {1}", userId, serverId));
                    }
                }

                string credentialHash = userServerInfo.CredentialHash;
                string url = server.Url;

                WeekInfo currentWeek = null;
                using (WeekInfoRepository weekInfoRepository = new WeekInfoRepository())
                {
                    currentWeek = weekInfoRepository.GetCurrentWeekInfo();
                    if (currentWeek == null)
                    {
                        throw new Exception("Current Week is not registered.");
                    }
                }
                List<WorkItem> tfsWorkItems = _teamServerManagementService.GetUserIncompleteItems(url, credentialHash);
                using (WorkItemRepository workItemRepository = new WorkItemRepository())
                {
                    foreach (WorkItem item in tfsWorkItems)
                    {
                        // Add new items.
                        UserWorkItem existingWorkItem = workItemRepository.Find(x => x.UserId == userId
                        && x.WeekId == currentWeek.Id
                        && x.TaskId == item.Id);
                        if (existingWorkItem == null)
                        {
                            UserWorkItem newWorkItem = item.ToEntity(userServerInfo.TfsId);
                            newWorkItem.UserId = userId;
                            newWorkItem.AssignedTo = userInfo.FirstName + " " + userInfo.LastName;
                            newWorkItem.WeekId = currentWeek.Id;
                            int newWorkItemId = workItemRepository.Insert(newWorkItem);

                            workItemsDtoList.Add(newWorkItem.ToDto(newWorkItemId));
                        }
                        else
                        {
                            existingWorkItem.Title = item.Title;
                            existingWorkItem.Status = item.State;
                            existingWorkItem.Sprint = item.IterationPath;
                            existingWorkItem.Project = item.AreaPath;
                            existingWorkItem.Description = item.Description;

                            workItemRepository.Update(existingWorkItem);
                            workItemsDtoList.Add(existingWorkItem.ToDto(existingWorkItem.Id));
                        }
                    }
                }

                return workItemsDtoList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public List<WorkItemDto> GetUserCurrentWeekSyncedTasks(string userId, bool includeIncompleteItems = true)
        {
            List<WorkItemDto> currentWeekTasks = new List<WorkItemDto>();
            WeekInfo currentWeek = null;

            UserInfo currentUser = null;
            using (UserInfoRepository userInfoRepository = new UserInfoRepository())
            {
                currentUser = userInfoRepository.Find(x => x.UserId == userId);
                if (currentUser == null)
                {
                    throw new Exception(string.Format("User Id {0} not found", userId));
                }
            }

            // Get current week.
            using (WeekInfoRepository repository = new WeekInfoRepository())
            {
                currentWeek = repository.GetCurrentWeekInfo();
                if (currentWeek == null)
                {
                    throw new Exception("Current Week is not registered.");
                }
            }

            // Get servers configured for the user.
            List<UserServerInfo> userServers = new List<UserServerInfo>();
            using (UserServerInfoRepository repository = new UserServerInfoRepository())
            {
                userServers = repository.Filter(x => x.UserId == userId).ToList();
            }

            // Get user incomplete tasks from task repository.
            List<int> currentWeekTaskIdList = new List<int>();
            List<int> currentWeekMovedTaskList = new List<int>();
            foreach (UserServerInfo server in userServers)
            {
                using (WorkItemRepository workItemRepository = new WorkItemRepository())
                {
                    IQueryable<UserWorkItem> currentWeekSavedTasks = workItemRepository
                        .Filter(x => x.ServerId == server.Id
                        && x.UserId == userId
                        && x.State != WorkItemState.Moved
                        && x.WeekId == currentWeek.Id);

                    IQueryable<UserWorkItem> currentWeekMovedTasks = workItemRepository
                        .Filter(x => x.ServerId == server.Id
                        && x.UserId == userId
                        && x.State == WorkItemState.Moved
                        && x.WeekId == currentWeek.Id);

                    currentWeekTaskIdList = currentWeekSavedTasks.Select(x => x.TaskId).ToList();
                    currentWeekMovedTaskList = currentWeekSavedTasks.Select(x => x.TaskId).ToList();
                }

                TeamServer teamServer = null;
                using (TeamServerRepository repository = new TeamServerRepository())
                {
                    teamServer = repository.Find(x => x.Id == server.TfsId);
                    if (teamServer == null)
                    {
                        continue;
                    }
                }

                // Get all tasks by id including to do items.
                List<WorkItem> userIncompleteItems = _teamServerManagementService
                    .GetWorkItemsByIds(currentWeekTaskIdList, teamServer.Url, server.CredentialHash, includeIncompleteItems);
                using (WorkItemRepository workItemRepository = new WorkItemRepository())
                {
                    foreach (WorkItem item in userIncompleteItems)
                    {
                        // Add new items.
                        UserWorkItem existingWorkItem = workItemRepository.Find(x => x.UserId == userId
                        && x.WeekId == currentWeek.Id
                        && x.TaskId == item.Id);
                        if (existingWorkItem == null)
                        {
                            UserWorkItem newWorkItem = item.ToEntity(server.TfsId);
                            newWorkItem.UserId = userId;
                            newWorkItem.AssignedTo = currentUser.FirstName + " " + currentUser.LastName;
                            newWorkItem.WeekId = currentWeek.Id;
                            int newWorkItemId = workItemRepository.Insert(newWorkItem);

                            currentWeekTasks.Add(newWorkItem.ToDto(newWorkItemId));
                        }
                        else
                        {
                            existingWorkItem.Title = item.Title;
                            existingWorkItem.Status = item.State;
                            existingWorkItem.Sprint = item.IterationPath;
                            existingWorkItem.Project = item.AreaPath;
                            existingWorkItem.Description = item.Description;

                            workItemRepository.Update(existingWorkItem);
                            currentWeekTasks.Add(existingWorkItem.ToDto(existingWorkItem.Id));
                        }
                    }
                }
            }
            return currentWeekTasks;
        }

        public List<WorkItemDto> GetUserSyncedTasksByDateRange(DateTime fromDate, DateTime endDate, string userId, bool includeIncompleteItems = false)
        {
            List<WorkItemDto> workItemDtos = new List<WorkItemDto>();
            if (fromDate.Date > DateTime.Today.Date.AddDays(7))
            {
                return workItemDtos;
            }

            if (endDate.Date < fromDate.Date)
            {
                return workItemDtos;
            }
            WeekInfo currentWeekInfo = null;
            List<int> weekIdRange = new List<int>();
            using (WeekInfoRepository repository = new WeekInfoRepository())
            {
                currentWeekInfo = repository.GetCurrentWeekInfo();
                if (currentWeekInfo == null)
                {
                    throw new Exception("Current Week is not registered.");
                }

                IQueryable<WeekInfo> weekItems = repository.FilterLocal(
                x => DbFunctions.TruncateTime(x.StartDate) >= fromDate
                && DbFunctions.TruncateTime(x.EndDate) <= endDate);

                weekIdRange = weekItems.Select(x => x.Id).ToList();
            }
            if (weekIdRange.Count == 0 && !includeIncompleteItems)
            {
                return workItemDtos;
            }

            // Get Current Week Tasks.
            if (weekIdRange.Contains(currentWeekInfo.Id))
            {
                List<WorkItemDto> userWorkItems = GetUserCurrentWeekSyncedTasks(userId, includeIncompleteItems);
                workItemDtos.AddRange(userWorkItems);
                weekIdRange = weekIdRange.Except(new List<int>() { currentWeekInfo.Id }).ToList();
            }

            // Get other week Tasks.
            using (WorkItemRepository repository = new WorkItemRepository())
            {
                List<WorkItemDto> otherWeekWorkItems =
                    repository.Filter(x => weekIdRange.Contains(x.WeekId)).ToList()
                    .Select(x => x.ToDto(x.Id)).ToList();
                workItemDtos.AddRange(otherWeekWorkItems);
            }

            return workItemDtos;
        }

        #endregion
    }
}