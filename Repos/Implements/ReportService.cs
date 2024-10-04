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
        public void CreateReport(CreateReportRequest request)
        {
            var report = new Report
            {
                WorkId = request.WorkId,
                Description = request.Description,
                Feed = request.Feed,
                Clean = request.Clean,
                HealthDescription = request.HealthDescription,
                DateTime = DateTime.UtcNow
            };

            _unitOfWork.ReportRepository.Insert(report);
            _unitOfWork.Save();
        }
    }
}
