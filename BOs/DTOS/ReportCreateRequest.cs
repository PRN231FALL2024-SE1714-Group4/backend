﻿using BOs.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTOS
{
    public class ReportCreateRequest
    {
        public Guid WorkId { get; set; } // ID of the work associated with the report
        public string Description { get; set; } // Report description
        //public string HealthDescription { get; set; } // Health description of the work
        //public DateTime DateTime { get; set; }
        public WorkStatus? WorkStatus { get; set; }
    }
}
