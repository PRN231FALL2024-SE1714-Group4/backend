using BOs.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BOs
{
    public class Work : BaseEntity
    {
        [Key]
        public Guid WorkId { get; set; } = Guid.NewGuid();

        public Guid CageID { get; set; }

        public Guid AssignerID { get; set; }

        public Guid AssigneeID { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WorkStatus Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WorkShiftEnum Shift { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WorkMission Mission { get; set; }

        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual User Assigner { get; set; }
        public virtual User Assignee { get; set; }
        public virtual Cage Cage { get; set; }
        [JsonIgnore]
        public virtual ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
