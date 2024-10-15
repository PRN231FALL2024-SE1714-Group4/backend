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
        List<UserShiftTimeResponse> getAvailableUserForSpecificShift(DateOnly fromDate, DateOnly toDate, WorkShiftEnum workShiftEnum);
        List<UserShift> getMyShift(DateOnly fromDate, DateOnly toDate);
        public bool editUserShift(Guid shiftId, UserShiftRequest updatedShiftRequest);
        public bool deleteUserShift(Guid shiftId);
        List<UserShift> getAllUserShifts(DateOnly fromDate, DateOnly toDate);
        UserShift getUserShiftById(Guid id);
    }
}
