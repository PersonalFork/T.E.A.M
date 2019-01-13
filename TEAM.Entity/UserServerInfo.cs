using System.ComponentModel.DataAnnotations.Schema;
using TEAM.Entity.Base;

namespace TEAM.Entity
{
    [Table("UserServerInfo")]
    public class UserServerInfo : EntityBase
    {
        public string UserId { get; set; }
        public int TfsId { get; set; }
        public string TfsUserId { get; set; }
        public string CredentialHash { get; set; }
    }
}
