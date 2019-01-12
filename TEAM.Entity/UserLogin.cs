using System;
using System.ComponentModel.DataAnnotations.Schema;
using TEAM.Entity.Base;

namespace TEAM.Entity
{
    [Table("UserLogin")]
    public class UserLogin : EntityBase
    {
        public int EmployeeId { get; set; }
        public string Password { get; set; }
        public bool IsLocked { get; set; }
        public int RetryCount { get; set; }
        public DateTime LastLoggedIn { get; set; }
    }
}
