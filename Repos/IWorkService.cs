using BOs;
using BOs.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IWorkService
    {
        void CreateWork(CreateWorkRequest request);
        IEnumerable<Work> ViewMyWork();
        IEnumerable<Work> ViewAssignedTasks();
    }
}
