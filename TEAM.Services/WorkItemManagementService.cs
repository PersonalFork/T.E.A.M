
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
        private readonly ITeamServerManagementService _teamServerManagementService;
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #region Constructor.

        public WorkItemManagementService()
        {
            _teamServerManagementService = new TeamServerManagementService();
        }

        #endregion

        #region IWorkItemManagementService Implementation.

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

        public List<WorkItemDto> GetUserIncompleteItems(int serverId, string userId)
        {
            TeamServer server = null;
            List<UserWorkItem> workItems = null;
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
                List<WorkItem> tfsWorkItems = _teamServerManagementService.GetUserIncompleteItems(url, credentialHash);
                workItems = tfsWorkItems.Select(x => x.ToEntity(serverId)).ToList();

                foreach (UserWorkItem workItem in workItems)
                {
                    workItemsDtoList.Add(workItem.ToDto(workItem.Id));
                }
                return workItemsDtoList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        public List<WorkItemDto> GetCurrentWeekTasks(string userId)
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
                // For MySql the below query needs to be executed.
                //CREATE FUNCTION `TruncateTime`(dateValue DateTime) RETURNS date
                //    return Date(dateValue)

                currentWeek = repository.Find(
                    x => DbFunctions.TruncateTime(x.StartDate) <= DateTime.Today
                    && DbFunctions.TruncateTime(x.EndDate) >= DateTime.Today
                    );
                if (currentWeek == null)
                {
                    throw new Exception("Week not found");
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
                List<WorkItem> userIncompleteItems = _teamServerManagementService.GetWorkItemsByIds(currentWeekTaskIdList, teamServer.Url, server.CredentialHash, true);
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

        private List<WorkItemDto> GetUpdatedItemsByWeekId(string userId, int weekId)
        {
            List<WorkItemDto> updatedWorkItems = new List<WorkItemDto>();
            IQueryable<IGrouping<int, UserWorkItem>> workItemGroups = null;
            List<UserWorkItem> existingWorkItem = new List<UserWorkItem>();

            // Get group of work items by Server Id.
            using (WorkItemRepository repository = new WorkItemRepository())
            {
                workItemGroups = repository.Filter
                    (x => x.UserId == userId && x.WeekId == weekId)
                    .GroupBy(x => x.ServerId);
            }

            // Loop for each configured server.
            foreach (IGrouping<int, UserWorkItem> group in workItemGroups)
            {
                int serverId = group.Key;
                IEnumerable<WorkItemDto> groupList = group.Select(x => x.ToDto(x.Id));
                List<WorkItemDto> workItems = GetUpdateWorkItems(group, userId);
                updatedWorkItems.AddRange(workItems);
            }

            return updatedWorkItems;
        }

        private List<WorkItemDto> GetUpdateWorkItems(IGrouping<int, UserWorkItem> group, string userId)
        {
            int serverId = group.Key;
            TeamServer server = null;
            List<WorkItem> workItems = null;
            List<WorkItemDto> workItemsDtoList = new List<WorkItemDto>();
            UserInfo userInfo = null;
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

                using (UserInfoRepository userInfoRepository = new UserInfoRepository())
                {
                    userInfo = userInfoRepository.Find(x => x.UserId == userId);
                    if (userInfo == null)
                    {
                        throw new Exception(string.Format("User with ID {0} Not Found", userId));
                    }
                }

                string formattedTaskQuery = string.Empty;

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

                    string credentialHash = userServerInfo.CredentialHash;
                    string url = server.Url;

                    //workItems = _teamServerManagementService.GetWorkItemByQuery(query, url, credentialHash);
                }
                foreach (WorkItem workItem in workItems)
                {
                    workItemsDtoList.Add(workItem.ToEntity(serverId).ToDto(workItem.Id));
                }
                return workItemsDtoList;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }

        #endregion
    }
}