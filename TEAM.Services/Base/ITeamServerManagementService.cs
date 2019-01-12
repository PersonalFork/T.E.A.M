using System;
using System.Collections.Generic;
using WorkItem = Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem;

namespace TEAM.Business.Base
{
    public interface ITeamServerManagementService
    {
        int AddTeamServer(string name, string url, int port);
        [Obsolete("This method has been marked as obsolete. " +
            "Please use Authenticate(int tfsId, string credentialHash) instead.")]
        bool Authenticate(string url, string userId, string password, string domain);
        WorkItem GetWorkItemById(int taskId, string serverUrl, string credentialHash);
        List<WorkItem> GetWorkItemByQuery(string query, string serverUrl, string credentialHash);
    }
}