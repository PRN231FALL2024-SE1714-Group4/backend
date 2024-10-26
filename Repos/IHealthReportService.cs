using BOs;
using BOs.DTOS;
using Repos.Response;
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
        void AddHealthReport(HealthReportCreateRequest HealthReportCreate);
        bool UpdateHealthReport(Guid id, HealthReportUpdateRequest healthReportUpdate);
        bool DeleteHealthReport(Guid id);
        List<CageNeedToReport> GetCageNeedToReport();
        List<HealthReport> GetHealthEeportByAnimal(Guid animalID);
    }
}
