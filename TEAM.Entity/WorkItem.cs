using TEAM.Entity.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace TEAM.Entity
{
    [Table("WorkItems")]
    public class WorkItem : EntityBase
    {
        public string TaskId { get; set; }
    }
}