using BOs;
using BOs.DTOS;
using DAOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HealthReportService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        List<BOs.HealthReport> IHealthReportService.GetAllHealthReports()
        {
            return _unitOfWork.HealthReportRepository.Get().ToList();
        }

        BOs.HealthReport IHealthReportService.GetHealthReportById(Guid id)
        {
            return _unitOfWork.HealthReportRepository
                .Get(filter: x => x.HelthReportID == id,
                    includeProperties: "Cage,User")
                .FirstOrDefault();
        }

        public void AddHealthReport(HealthReportCreateRequest healthReportCreate)
        {
            if (healthReportCreate == null)
                throw new ArgumentNullException(nameof(healthReportCreate));

            var healthReport = new BOs.HealthReport()
            {
                CageID = healthReportCreate.CageID,
                Status = healthReportCreate.Status,
                DateTime = healthReportCreate.DateTime,
                Description = healthReportCreate.Description,
                //WorkShift = healthReportCreate.WorkShift,
                UserID = this.GetCurrentUserId(),
            };

            _unitOfWork.HealthReportRepository.Insert(healthReport);
            _unitOfWork.Save();
        }

        public bool UpdateHealthReport(Guid id, HealthReportUpdateRequest healthReportUpdate)
        {
            if (healthReportUpdate == null)
                throw new ArgumentNullException(nameof(healthReportUpdate));

            var existingHealthReport = _unitOfWork.HealthReportRepository.GetByID(id);

            existingHealthReport.Description = healthReportUpdate.Description ?? existingHealthReport.Description;
            existingHealthReport.DateTime = healthReportUpdate.DateTime ?? existingHealthReport.DateTime;
            existingHealthReport.Status = healthReportUpdate?.Status ?? existingHealthReport.Status;
            //existingHealthReport.WorkShift = healthReportUpdate?.WorkShift ?? existingHealthReport.WorkShift;


            bool isUpdated = _unitOfWork.HealthReportRepository.Update(id, existingHealthReport);
            if (isUpdated)
            {
                _unitOfWork.Save();
            }
            return isUpdated;
        }

        public bool DeleteHealthReport(Guid id)
        {
            bool isDeleted = _unitOfWork.HealthReportRepository.Delete(id);
            if (isDeleted)
            {
                _unitOfWork.Save();
            }
            return isDeleted;
        }

        public Guid GetCurrentUserId()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null)
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("nameid");

                if (userIdClaim != null)
                {
                    return Guid.Parse(userIdClaim.Value);
                }
            }

            throw new Exception("User ID not found.");
        }
    }
}
