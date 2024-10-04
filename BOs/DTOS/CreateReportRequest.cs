using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTOS
{
    public class CreateReportRequest
    {
        public Guid WorkId { get; set; } // ID of the work associated with the report
        public string Description { get; set; } // Report description
        public bool Feed { get; set; } // Indicates if feeding was done
        public bool Clean { get; set; } // Indicates if cleaning was done
        public string HealthDescription { get; set; } // Health description of the work
    }
}
