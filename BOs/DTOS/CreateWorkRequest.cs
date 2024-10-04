using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTOS
{
    public class CreateWorkRequest
    {
        public Guid RoleID { get; set; } // Role ID for the assignee
        public Guid AreaID { get; set; } // Area where the work is assigned
        public string Description { get; set; } // Task description
        public DateTime StartDate { get; set; } // Start date of the task
        public DateTime EndDate { get; set; } // End date of the task
        public string Shift { get; set; } // Shift assigned for the task
        public Guid AssigneeID { get; set; }
    }

}
