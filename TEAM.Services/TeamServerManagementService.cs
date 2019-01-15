using Microsoft.TeamFoundation;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;

using Newtonsoft.Json;

using NLog;

using System;
using System.Collections.Generic;
using System.Net;

using TEAM.Business.Base;
using TEAM.Common;
using TEAM.Common.Exceptions;
using TEAM.DAL.Repositories;
using TEAM.Entity;

using WindowsCredential = Microsoft.VisualStudio.Services.Common.WindowsCredential;
using WorkItem = Microsoft.TeamFoundation.WorkItemTracking.Client.WorkItem;

namespace TEAM.Business
{
    public class TeamServerManagementService : ITeamServerManagementService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region ITeamServerManagementService Implementation.

        #region Team Server Management.

        public int DeleteTeamServer(int id)
        {
            using (TeamServerRepository repository = new TeamServerRepository())
            {
                TeamServer server = repository.GetById(id);
                if (server != null)
                {
                    return repository.Delete(server);
                }
                else
                {
                    throw new Exception(string.Format("Server with specifed id {0} does not exist.", id));
                }
            }
        }

        public int AddTeamServer(string name, string url, int port)
        {
            try
            {
                TeamServer server = new TeamServer
                {
                    Name = name,
                    Url = url.ToLower().Trim(),
                    Port = port
                };

                using (TeamServerRepository repository = new TeamServerRepository())
                {
                    TeamServer ts = repository.Find(x => x.Url == server.Url);
                    if (ts == null)
                    {
                        return repository.Insert(server);
                    }
                    else
                    {
                        throw new Exception("Server already exists.");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        #endregion

        #region Authentication.

        public bool Authenticate(int tfsId, string credentialHash)
        {
            TeamServer teamServer = null;
            string url = string.Empty;
            try
            {
                using (TeamServerRepository teamServerRepository = new TeamServerRepository())
                {
                    teamServer = teamServerRepository.GetById(tfsId);
                    if (teamServer == null)
                    {
                        throw new Exception(string.Format("Invalid Team Server Id : {0}", tfsId));
                    }
                }
                url = teamServer.Url;
                NetworkCredential credential = JsonConvert.DeserializeObject<NetworkCredential>(credentialHash.Decrypt());
                WindowsCredential winCred = new WindowsCredential(credential);
                VssCredentials vssCred = new VssClientCredentials(winCred)
                {
                    PromptType = CredentialPromptType.DoNotPrompt
                };
                using (TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri(url), vssCred))
                {
                    tpc.EnsureAuthenticated();
                    return true;
                }
            }
            catch (TeamFoundationServiceUnavailableException ex)
            {
                logger.Error(ex, "Service not available");
                throw new ServiceUnavailableException(url, "Service not Available", ex);
            }
            catch (TeamFoundationServerUnauthorizedException ex)
            {
                logger.Error(ex, "Authentication Failure.");
                throw new TFSAuthenticationException("Authentication Failure.", ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Authentication Failure.");
                throw;
            }
        }

        [Obsolete("This method has been marked as obsolete. " +
            "Use Authenticate(int tfsId, string credentialHash) instead.")]
        public bool Authenticate(string url, string userId, string password, string domain)
        {
            try
            {
                NetworkCredential credential = new NetworkCredential(userId, password, domain);
                WindowsCredential winCred = new WindowsCredential(credential);
                VssCredentials vssCred = new VssClientCredentials(winCred)
                {
                    PromptType = CredentialPromptType.DoNotPrompt
                };
                using (TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri(url), vssCred))
                {
                    tpc.EnsureAuthenticated();
                    return true;
                }
            }
            catch (TeamFoundationServiceUnavailableException ex)
            {
                logger.Error(ex, "Service not available");
                throw new ServiceUnavailableException(url, "Service not Available", ex);
            }
            catch (TeamFoundationServerUnauthorizedException ex)
            {
                logger.Error(ex, "Authentication Failure.");
                throw new TFSAuthenticationException("Authentication Failure.", ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Authentication Failure.");
                throw;
            }
        }

        #endregion

        #region Task Management.

        /// <summary>
        /// Retrieves a Work Item by ID from the configured server.
        /// </summary>
        /// <param name="taskId">Task Id.</param>
        /// <param name="serverUrl">Server URL.</param>
        /// <param name="credentialHash">The hash for authentication.</param>
        /// <returns></returns>
        public WorkItem GetWorkItemById(int taskId, string serverUrl, string credentialHash)
        {
            WorkItemCollection col = null;
            try
            {
                NetworkCredential credential = JsonConvert.DeserializeObject<NetworkCredential>(credentialHash.Decrypt());
                WindowsCredential winCred = new WindowsCredential(credential);
                VssCredentials vssCred = new VssClientCredentials(winCred);
                TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri(serverUrl), vssCred);

                string query = "Select * From WorkItems Where [Id] = '" + taskId + "' " + "Order By [State] Asc, [Changed Date] Desc";
                WorkItemStore wis = new WorkItemStore(tpc);
                col = wis.Query(query);
                if (col.Count > 0)
                {
                    return col[0];
                }
                else
                {
                    return null;
                }
            }
            catch (TeamFoundationServiceUnavailableException ex)
            {
                logger.Error(ex, "Service not available");
                throw new ServiceUnavailableException(serverUrl, "Service not Available", ex);
            }
            catch (TeamFoundationServerUnauthorizedException ex)
            {
                logger.Error(ex, "Authentication Failure.");
                throw new TFSAuthenticationException("Authentication Failure.", ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Authentication Failure.");
                throw;
            }
        }

        public List<WorkItem> GetWorkItemByQuery(string query, string serverUrl, string credentialHash)
        {
            WorkItemCollection col = null;
            try
            {
                NetworkCredential credential = JsonConvert.DeserializeObject<NetworkCredential>(credentialHash.Decrypt());
                WindowsCredential winCred = new WindowsCredential(credential);
                VssCredentials vssCred = new VssClientCredentials(winCred);
                TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri(serverUrl), vssCred);

                if (query == null)
                {
                    throw new Exception("Query cannot be empty");
                }

                WorkItemStore wis = new WorkItemStore(tpc);
                col = wis.Query(query);
                List<WorkItem> wil = new List<WorkItem>();
                for (int i = 0; i < col.Count; i++)
                {
                    wil.Add(col[i]);
                }
                return wil;
            }
            catch (TeamFoundationServiceUnavailableException ex)
            {
                logger.Error(ex, "Service not available");
                throw new ServiceUnavailableException(serverUrl, "Service not Available", ex);
            }
            catch (TeamFoundationServerUnauthorizedException ex)
            {
                logger.Error(ex, "Authentication Failure.");
                throw new TFSAuthenticationException("Authentication Failure.", ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Authentication Failure.");
                throw;
            }
        }

        public List<WorkItem> GetWorkItemByIds(List<int> taskIdList, string serverUrl, string credentialHash)
        {
            WorkItemCollection col = null;
            try
            {
                NetworkCredential credential = JsonConvert.DeserializeObject<NetworkCredential>(credentialHash.Decrypt());
                WindowsCredential winCred = new WindowsCredential(credential);
                VssCredentials vssCred = new VssClientCredentials(winCred);
                TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(new Uri(serverUrl), vssCred);

                if (taskIdList == null || taskIdList.Count == 0)
                {
                    throw new Exception("Task List cannot be empty");
                }

                string iterationQuery = "'" + taskIdList[0].ToString() + "' ";
                foreach (int taskId in taskIdList)
                {
                    iterationQuery = "OR '" + taskId + "' ";
                }

                string query = "Select * From WorkItems Where [Id] = "
                    + iterationQuery
                    + "Order By [State] Asc, [Changed Date] Desc";

                WorkItemStore wis = new WorkItemStore(tpc);
                col = wis.Query(query);
                List<WorkItem> wil = new List<WorkItem>();
                for (int i = 0; i < col.Count; i++)
                {
                    wil.Add(wil[i]);
                }
                return wil;
            }
            catch (TeamFoundationServiceUnavailableException ex)
            {
                logger.Error(ex, "Service not available");
                throw new ServiceUnavailableException(serverUrl, "Service not Available", ex);
            }
            catch (TeamFoundationServerUnauthorizedException ex)
            {
                logger.Error(ex, "Authentication Failure.");
                throw new TFSAuthenticationException("Authentication Failure.", ex);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Authentication Failure.");
                throw;
            }
        }
        #endregion 

        #endregion
    }
}