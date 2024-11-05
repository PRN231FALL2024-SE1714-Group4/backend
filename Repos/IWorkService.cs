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
        WorkResponse CreateWork(WorkCreateRequest request);
        WorkResponse UpdateWork(Guid id, WorkUpdateRequest request);
        IEnumerable<WorkResponse> ViewMyWork();
        IEnumerable<WorkResponse> ViewAssignedTasks();
        WorkResponse GetWorkByID(Guid id);
        List<WorkResponse> GetWorks();
        bool DeleteWork(Guid id);

        List<WorkResponse> GetActiveWorksForToday();
        List<WorkResponse> GetMyWorkToday();

        IQueryable<WorkResponse> GetWorksOdata();
    }
}
