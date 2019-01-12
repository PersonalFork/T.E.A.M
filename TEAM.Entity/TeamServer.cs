using System.ComponentModel.DataAnnotations.Schema;
using TEAM.Entity.Base;

namespace TEAM.Entity
{
    [Table("TeamServers")]
    public class TeamServer : EntityBase
    {
        public string Name { get; set; }
        public int Port { get; set; }
        public string Url { get; set; }
    }
}