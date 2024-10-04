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
        void CreateReport(CreateReportRequest request);
    }
}
