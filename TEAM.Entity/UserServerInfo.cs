using System.ComponentModel.DataAnnotations.Schema;
using TEAM.Entity.Base;

namespace TEAM.Entity
{
    [Table("UserServerInfo")]
    public class UserServerInfo : EntityBase
    {
        public int EmployeeId { get; set; }
        public int TfsId { get; set; }
        public string UserId { get; set; }
        public string CredentialHash { get; set; }
    }
}
