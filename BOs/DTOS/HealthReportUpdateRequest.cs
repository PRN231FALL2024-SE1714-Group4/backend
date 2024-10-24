using BOs.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTOS
{
    public class HealthReportUpdateRequest
    {
        public string? Description { get; set; }
        //public WorkShiftEnum? WorkShift { get; set; }
        public DateTime? DateTime { get; set; }
        public HealthStatus? Status { get; set; }
    }
}
