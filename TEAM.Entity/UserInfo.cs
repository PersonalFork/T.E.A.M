using System.ComponentModel.DataAnnotations.Schema;
using TEAM.Entity.Base;

namespace TEAM.Entity
{
    [Table("UserInfo")]
    public class UserInfo : EntityBase
    {
        public int EmployeeId { get; set; }
        public string EMail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int ManagerId { get; set; }
    }
}
