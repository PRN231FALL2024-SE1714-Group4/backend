using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IHealthReportService
    {
        List<HealthReport> GetAllHealthReports();
        HealthReport GetHealthReportById(Guid id);
        void AddHealthReport(HealthReport healthReport);
        bool UpdateHealthReport(Guid id, HealthReport healthReport);
        bool DeleteHealthReport(Guid id);
    }
}
