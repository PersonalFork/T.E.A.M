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
using TEAM.WebAPI.Filters;

namespace TEAM.WebAPI.Controllers
{
    [RoutePrefix("api/userMgmt")]
    public class UserManagementController : ApiController
    {
        public static readonly ILog _loggger = LogManager.GetLogger(typeof(UserManagementController));
        private readonly IUserManagementService _userManagementService;

        public UserManagementController()
        {
            _userManagementService = new UserManagementService();
        }

        [HttpGet]
        [Route("getServersByUserId")]
        [Authentication]
        public HttpResponseMessage GetServersByUserId()
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
                List<UserServerDto> serverList = _userManagementService.GetUserServerList(userId);
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, serverList);
                return response;
            }
            catch (Exception ex)
            {
                _loggger.Error(ex);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}