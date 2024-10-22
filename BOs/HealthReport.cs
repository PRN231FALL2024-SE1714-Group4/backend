using BOs.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs
{
    public class HealthReport
    {
        [Key]
        public Guid HelthReportID { get; set; } = Guid.NewGuid();
        public Guid CageID { get; set; }
        public Guid UserID { get; set; }
        public string Description {  get; set; }
        public WorkShiftEnum WorkShift { get; set; }
        public DateTime DateTime { get; set; }
        public HealthStatus Status { get; set; }
        

        public virtual Cage Cage { get; set; }
        public virtual User User { get; set; }
    }
}
