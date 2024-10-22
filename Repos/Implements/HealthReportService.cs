using BOs;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Implements
{
    public class HealthReportService : IHealthReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HealthReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        List<HealthReport> IHealthReportService.GetAllHealthReports()
        {
            return _unitOfWork.HealthReportRepository.Get().ToList();
        }

        HealthReport IHealthReportService.GetHealthReportById(Guid id)
        {
            return _unitOfWork.HealthReportRepository.GetByID(id);
        }

        public void AddHealthReport(HealthReport healthReport)
        {
            if (healthReport == null)
                throw new ArgumentNullException(nameof(healthReport));

            _unitOfWork.HealthReportRepository.Insert(healthReport);
            _unitOfWork.Save();
        }

        public bool UpdateHealthReport(Guid id, HealthReport healthReport)
        {
            bool isDeleted = _unitOfWork.HealthReportRepository.Delete(id);
            if (isDeleted)
            {
                _unitOfWork.Save();
            }
            return isDeleted;
        }

        public bool DeleteHealthReport(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
