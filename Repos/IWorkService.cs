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
    public interface IWorkService
    {
        WorkResponse CreateWork(CreateWorkRequest request);
        IEnumerable<WorkResponse> ViewMyWork();
        IEnumerable<WorkResponse> ViewAssignedTasks();
        WorkResponse GetWorkByID(Guid id);
        List<Work> GetWorks();
    }
}
