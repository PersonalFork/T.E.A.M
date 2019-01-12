using System;
using System.ComponentModel.DataAnnotations.Schema;
using TEAM.Entity.Base;

namespace TEAM.Entity
{
    [Table("UserSessions")]
    public class UserSession : EntityBase
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime LastAccessed { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
