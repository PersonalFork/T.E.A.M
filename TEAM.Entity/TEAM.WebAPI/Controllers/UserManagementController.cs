using System;
using System.Web.Http;
using NEC.TEAM.Common.Interfaces;
using NEC.TEAM.WebAPI.Models;

namespace NEC.TEAM.Web.Controllers
{
    [RoutePrefix("team/userManagement")]
    public class UserManagementController : ApiController, IUserManagementController
    {
        [Route("Test", Name = "Test")]
        [HttpGet]
        public string Test()
        {
            return String.Format("{0} {1} ", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
        }

        [Route("Register", Name = "RegisterUser")]
        [HttpPost]
        public string RegisterUser(UserDto user)
        {
            return String.Format("{0} {1} ", user.FirstName, user.LastName);
        }
    }
}
