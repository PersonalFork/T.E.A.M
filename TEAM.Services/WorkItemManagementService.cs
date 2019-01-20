using NLog;

using System;
using System.Collections.Generic;

using TEAM.Business.Base;
using TEAM.Business.Dto;
using TEAM.Business.Extensions;
using TEAM.DAL.Repositories;
using TEAM.Entity;

using WorkItem = Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem;

namespace TEAM.Business
{
    public class WorkItemManagementService : IWorkItemManagementService
    {
        private readonly ITeamServerManagementService _teamServerManagementService;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

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
                    userServerInfo = userServerInfoRepository.FindLocal(x => x.UserId != null && x.UserId.ToUpper() == userId.ToUpper()
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
                        return workItem.ToDto(serverId);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public List<WorkItemDto> GetCurrentUserIncompleteItems(int serverId, string userId)
        {
            TeamServer server = null;
            List<WorkItem> workItems = null;
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
                    string credentialHash = userServerInfo.CredentialHash;
                    string url = server.Url;
                    string query = "Select * From WorkItems " +
                        "Where [System.AssignedTo] = @me " +
                        "AND ([System.State] = 'To Do' OR [System.State] = 'In Progress')"
                        + "Order By [State] Asc, [Changed Date] Desc";

                    workItems = _teamServerManagementService.GetWorkItemByQuery(query, url, credentialHash);
                }
                foreach (WorkItem workItem in workItems)
                {
                    workItemsDtoList.Add(workItem.ToDto(serverId));
                }
                return workItemsDtoList;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public List<WorkItemDto> Sync(int weekId, List<WorkItemDto> workItems, string userId)
        {
            List<WorkItemDto> currentTaskList = new List<WorkItemDto>();
            foreach (WorkItemDto item in workItems)
            {
                WorkItemDto workItem = GetWorkItemById(item.TaskId, item.ServerId, userId);
                if (workItem == null)
                {
                    AddWorkItem(workItem, userId);
                }
                else
                {
                    if (!workItem.Equals(item, out bool isUpdated) && isUpdated)
                    {

                    }
                }
            }
            return currentTaskList;
        }

        #endregion

        public bool AddWorkItem(WorkItemDto dto, string userId)
        {
            return false;
        }

        public bool UpdateWorkItem(WorkItemDto dto, string userId)
        {
            return false;
        }
    }
}