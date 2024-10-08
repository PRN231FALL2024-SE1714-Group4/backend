using BOs;
using BOs.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IReportService
    {
        Report CreateReport(ReportCreateRequest request);
        Report UpdateReport(Guid reportId, ReportUpdateRequest request);
        Report GetReportById(Guid reportId);
        IEnumerable<Report> GetReports(Guid? workId = null);
        bool DeleteReport(Guid reportId);
    }
}
