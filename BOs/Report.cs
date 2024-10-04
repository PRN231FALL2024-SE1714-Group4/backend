using BOs.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs
{
    public class Report : BaseEntity
    {
        [Key]
        public Guid ReportID { get; set; } = Guid.NewGuid();
        public Guid CageID { get; set; }
        public Guid WorkId { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public bool Feed { get; set; }
        public bool Clean { get; set; }
        public string HealthDescription { get; set; }
        public ReportStatus Status { get; set; }

        public virtual Cage Cage { get; set; }
        public virtual Work Work { get; set; }
    }
}
