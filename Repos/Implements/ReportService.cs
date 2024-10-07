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
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // 3. Function to create a report for a specific Work
        public Report CreateReport(CreateReportRequest request)
        {
            var report = new Report
            {
                WorkId = request.WorkId,
                Description = request.Description,
                HealthDescription = request.HealthDescription,
                DateTime = DateTime.UtcNow
            };

            _unitOfWork.ReportRepository.Insert(report);
            _unitOfWork.Save();
            return report;
        }

        public Report UpdateReport(Guid reportId, CreateReportRequest request)
        {
            var report = _unitOfWork.ReportRepository.GetByID(reportId);
            if (report == null)
                throw new Exception("Report not found.");

            report.Description = request.Description;
            report.HealthDescription = request.HealthDescription;
            report.DateTime = DateTime.UtcNow;

            _unitOfWork.ReportRepository.Update(report);
            _unitOfWork.Save();
            return report;
        }

        public Report GetReportById(Guid reportId)
        {
            var report = _unitOfWork.ReportRepository.GetByID(reportId);
            if (report == null)
                throw new Exception("Report not found.");

            return report;
        }

        public IEnumerable<Report> GetReports(Guid? workId = null)
        {
            if (workId.HasValue)
                return _unitOfWork.ReportRepository.Get(filter: r => r.WorkId == workId.Value).ToList();
            else
                return _unitOfWork.ReportRepository.Get().ToList();
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
    }
}
