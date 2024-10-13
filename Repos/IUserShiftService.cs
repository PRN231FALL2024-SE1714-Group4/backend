    using BOs;
    using BOs.DTOS;
using BOs.Enum;
using Repos.Response;
using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Repos
    {
        public interface IUserShiftService
        {
            bool createUserShift(List<UserShiftRequest> userShiftRequests);
            List<WorkerInShiftResponse> getAllWorkerInShift(DateOnly fromDate, DateOnly toDate);
            List<User> getAvailableUserForSpecificShift(DateOnly fromDate, DateOnly toDate, WorkShiftEnum workShiftEnum);
        }
    }
