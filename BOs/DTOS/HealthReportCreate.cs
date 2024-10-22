using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTOS
{
    public class HealthReportCreate
    {
        public Guid CageID { get; set; }
        public string Description { get; set; }
    }
}
