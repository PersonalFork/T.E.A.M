using System;
using System.Collections.Generic;
using System.Linq;
using TEAM.Business.Base;
using TEAM.Business.Dto;
using TEAM.Business.Extensions;
using TEAM.DAL.Repositories;
using TEAM.Entity;
using NLog;
using WorkItem = Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem;

namespace TEAM.Business
{
    public class WorkItemManagementService : IWorkItemManagementService
    {
        private readonly ITeamServerManagementService _teamServerManagementService;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public WorkItemManagementService()
        {
            _teamServerManagementService = new TeamServerManagementService();
        }

        public WorkItemDto GetWorkItemById(int taskId, int serverId, int employeeId)
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
                    userInfo = userInfoRepository.Find(x => x.EmployeeId == employeeId);
                    if (userInfo == null)
                    {
                        throw new Exception(string.Format("User with Employee ID {0} Not Found", employeeId));
                    }
                }

                using (UserServerInfoRepository userServerInfoRepository = new UserServerInfoRepository())
                {
                    userServerInfo = userServerInfoRepository.FindLocal(x => x.EmployeeId == employeeId && x.TfsId == serverId);
                    if (userServerInfo == null)
                    {
                        throw new Exception(string.Format("User with employee id : {0} is not registered with server id : {1}", employeeId, serverId));
                    }
                    string credentialHash = userServerInfo.CredentialHash;
                    string url = server.Url;
                    WorkItem workItem = _teamServerManagementService.GetWorkItemById(taskId, url, credentialHash);
                    if (workItem != null)
                    {
                        return workItem.ToDto();
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

        public List<WorkItemDto> GetCurrentUserIncompleteItems(int serverId, int employeeId)
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
                    userInfo = userInfoRepository.Find(x => x.EmployeeId == employeeId);
                    if (userInfo == null)
                    {
                        throw new Exception(string.Format("User with Employee ID {0} Not Found", employeeId));
                    }
                }

                using (UserServerInfoRepository userServerInfoRepository = new UserServerInfoRepository())
                {
                    userServerInfo = userServerInfoRepository.FindLocal(x => x.EmployeeId == employeeId && x.TfsId == serverId);
                    if (userServerInfo == null)
                    {
                        throw new Exception(string.Format("User with employee id : {0} is not registered with server id : {1}", employeeId, serverId));
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
                    workItemsDtoList.Add(workItem.ToDto());
                }
                return workItemsDtoList;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public List<WorkItemDto> GetIncompleteItemsByUserIdList(int serverId, int employeeId, List<int> employeeIdList)
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
                    userInfo = userInfoRepository.Find(x => x.EmployeeId == employeeId);
                    if (userInfo == null)
                    {
                        throw new Exception(string.Format("User with Employee ID {0} Not Found", employeeId));
                    }
                }
                List<string> serverUserIdList = new List<string>();
                using (UserServerInfoRepository userServerInfoRepository = new UserServerInfoRepository())
                {
                    userServerInfo = userServerInfoRepository.FindLocal(x => x.EmployeeId == employeeId && x.TfsId == serverId);
                    if (userServerInfo == null)
                    {
                        throw new Exception(string.Format("User with employee id : {0} is not registered with server id : {1}", employeeId, serverId));
                    }

                    string test = userServerInfoRepository.Filter(x => employeeIdList.Contains(x.EmployeeId))
                        .Select(x => x.UserId).Aggregate((current, next) => "'" + current + "' OR ");

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
                    workItemsDtoList.Add(workItem.ToDto());
                }
                return workItemsDtoList;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}