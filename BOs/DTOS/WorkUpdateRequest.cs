using BOs.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTOS
{
    public class WorkUpdateRequest
    {
        public string? Description { get; set; } // Task description
        public DateTime? StartDate { get; set; } // Start date of the task
        public DateTime? EndDate { get; set; } // End date of the task
        public WorkShift? Shift { get; set; } // Shift assigned for the task
        public Guid? AssigneeID { get; set; }
        public WorkMission? Mission { get; set; }
        public WorkStatus? Status { get; set; }
    }
}
