using BOs.DTOS;
using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOs;
using Microsoft.AspNetCore.Http;

namespace Repos.Implements
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // 3. Function to create a report for a specific Work
        public Report CreateReport(ReportCreateRequest request)
        {
            var workReport = _unitOfWork.WorkRepository.Get(filter: w => w.WorkId == request.WorkId).FirstOrDefault();
            if(workReport == null)
            {
                throw new Exception("There are no Work");
            }

            if(workReport.AssigneeID != this.GetCurrentUserId())
            {
                throw new Exception("You don't have permission to report this Work");
            }

            var report = new Report
            {
                WorkId = request.WorkId,
                Description = request.Description,
                HealthDescription = request.HealthDescription,
                DateTime = DateTime.UtcNow
            };

            _unitOfWork.ReportRepository.Insert(report);
            _unitOfWork.Save();
            return _unitOfWork.ReportRepository
                .Get(   filter: r => r.ReportID == report.ReportID,
                        includeProperties: "Work")
                .FirstOrDefault();
        }

        public Report UpdateReport(Guid reportId, ReportUpdateRequest request)
        {
            var report = _unitOfWork.ReportRepository.GetByID(reportId);
            if (report == null)
                throw new Exception("Report not found.");

            var workReport = _unitOfWork.WorkRepository.Get(filter: w => w.WorkId == report.WorkId).FirstOrDefault();
            if (workReport.AssigneeID != this.GetCurrentUserId())
            {
                throw new Exception("You don't have permission to report this Work");
            }

            report.Description = request.Description ?? report.Description;
            report.HealthDescription = request.HealthDescription ?? report.HealthDescription;
            //report.DateTime = DateTime.UtcNow ?? report;

            _unitOfWork.ReportRepository.Update(report);
            _unitOfWork.Save();
            return _unitOfWork.ReportRepository
                .Get(filter: r => r.ReportID == report.ReportID,
                        includeProperties: "Work")
                .FirstOrDefault();
        }

        public Report GetReportById(Guid reportId)
        {
            var report = _unitOfWork.ReportRepository.GetByID(reportId);
            if (report == null)
                throw new Exception("Report not found.");

            return _unitOfWork.ReportRepository
                .Get(filter: r => r.ReportID == report.ReportID,
                        includeProperties: "Work")
                .FirstOrDefault();
        }

        public IEnumerable<Report> GetReports(Guid? workId = null)
        {
            if (workId.HasValue)
                return _unitOfWork.ReportRepository
                    .Get(filter: r => r.WorkId == workId.Value,
                        includeProperties: "Work")
                    .ToList();
            else
                return _unitOfWork.ReportRepository
                    .Get(includeProperties: "Work")
                    .ToList();
        }

        public bool DeleteReport(Guid reportId)
        {
            var report = _unitOfWork.ReportRepository.GetByID(reportId);
            if (report == null)
                throw new Exception("Report not found.");

            _unitOfWork.ReportRepository.Delete(report);
            _unitOfWork.Save();
            return true;
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
