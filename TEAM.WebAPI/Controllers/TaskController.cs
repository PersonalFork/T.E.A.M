using log4net;

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using TEAM.Business;
using TEAM.Business.Base;
using TEAM.Business.Dto;
using TEAM.WebAPI.Common;

namespace TEAM.Web.Controllers
{
    [RoutePrefix("api/tasks")]
    public class TaskController : ApiController
    {
        public static readonly ILog _loggger = LogManager.GetLogger(typeof(TaskController));
        private readonly IWorkItemManagementService _workItemManagementService;
        public TaskController()
        {
            _workItemManagementService = new WorkItemManagementService();
        }

        [Route("getIncompleteTasks")]
        [HttpGet]
        public HttpResponseMessage GetIncompleteTasks(int serverId)
        {
            UserSessionDto userSession = Request.GetUserSession();
            if (userSession == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "User Session not found.");
            }

            string userId = userSession.User.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User Id cannot be empty.");
            }
            try
            {
                List<WorkItemDto> incompleteWorkItems = _workItemManagementService.GetCurrentUserIncompleteItems(serverId, userId);
                return Request.CreateResponse(HttpStatusCode.OK, incompleteWorkItems);
            }
            catch (Exception ex)
            {
                _loggger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
