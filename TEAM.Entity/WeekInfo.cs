using System;
using System.ComponentModel.DataAnnotations.Schema;

using TEAM.Entity.Base;

namespace TEAM.Entity
{
    [Table("WeekInfo")]
    public class WeekInfo : EntityBase
    {
        public string Label { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
