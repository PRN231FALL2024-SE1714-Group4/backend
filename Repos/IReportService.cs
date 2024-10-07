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
        Report CreateReport(CreateReportRequest request);
        Report UpdateReport(Guid reportId, CreateReportRequest request);
        Report GetReportById(Guid reportId);
        IEnumerable<Report> GetReports(Guid? workId = null);
        bool DeleteReport(Guid reportId);
    }
}
