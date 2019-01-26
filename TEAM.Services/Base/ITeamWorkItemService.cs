using Microsoft.TeamFoundation.WorkItemTracking.Client;

using System;
using System.Collections.Generic;

namespace TEAM.Business.Base
{
    public interface ITeamWorkItemService
    {
        #region Authentication.

        int AddTeamServer(string name, string url, int port);

        bool Authenticate(int tfsId, string credentialHash);

        [Obsolete("This method has been marked as obsolete. " +
            "Please use Authenticate(int tfsId, string credentialHash) instead.")]
        bool Authenticate(string url, string userId, string password, string domain);

        #endregion

        #region Work Item Queries.

        WorkItem GetWorkItemById(int taskId, string serverUrl, string credentialHash);

        List<WorkItem> GetUserIncompleteItems(string serverUrl, string credentialHash);

        List<WorkItem> GetWorkItemsByIds(IList<int> workItemId, string serverUrl, string credentialHash, bool includeIncompleteItems);

        List<WorkItem> GetWorkItemsByIds(IList<int> workItemId, IList<int> excludeIds, string serverUrl, string credentialHash, bool includeIncompleteItems);

        List<WorkItem> GetWorkItemByQuery(string query, string serverUrl, string credentialHash);

        #endregion
    }
}