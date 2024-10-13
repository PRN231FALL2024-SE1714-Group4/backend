using BOs.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTOS
{
    public class WorkCreateRequest
    {
        [Required]
        public Guid CageId { get; set; } // Area where the work is assigned
        public string Description { get; set; } // Task description
        [Required]
        public DateTime StartDate { get; set; } // Start date of the task
        [Required]
        public DateTime EndDate { get; set; } // End date of the task
        [Required]
        public WorkShiftEnum Shift { get; set; } // Shift assigned for the task
        [Required]
        public Guid AssigneeID { get; set; }
        [Required]
        public WorkMission Mission { get; set; }
    }

}
