using BOs;
using BOs.DTOS;
using BOs.Enum;
using DAOs;
using Microsoft.AspNetCore.Http;
using Repos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Implements
{
    public class UserShiftService : IUserShiftService
    {
        public IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserShiftService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
             _httpContextAccessor = httpContextAccessor;
        }

        public bool createUserShift(List<UserShiftRequest> userShiftRequests)
        {
            foreach (var shift in userShiftRequests)
            {
                var userShift = new UserShift()
                {
                    UserId = shift.UserId ?? this.GetCurrentUserId(),
                    EndDate = shift.EndDate.ToDateTime(new TimeOnly(23, 59)),
                    StartDate = shift.StartDate.ToDateTime(new TimeOnly(0, 0)),
                    WorkShift = shift.WorkShift
                };

                if (!this.checkSuitableTime(userShift))
                {
                    throw new Exception("Invalid Time");
                }

                if (this.checkDuplicated(userShift))
                {
                    throw new Exception("Duplicated Shift!");
                }
                _unitOfWork.UserShiftRepository.Insert(userShift);
            }
            _unitOfWork.Save();
            return true;
        }

        public List<WorkerInShiftResponse> getAllWorkerInShift(DateOnly fromDate, DateOnly toDate)
        {
            // Create a list to store the worker shift responses
            var workersInShifts = new List<WorkerInShiftResponse>();

            // Loop through each day from 'fromDate' to 'toDate'
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                // Convert the current 'DateOnly' to 'DateTime' for querying
                var fromDateTime = date.ToDateTime(new TimeOnly(0, 0));  // Start of the day
                var toDateTime = date.ToDateTime(new TimeOnly(23, 59));  // End of the day

                // Fetch all shifts for this specific day
                var shifts = _unitOfWork.UserShiftRepository.Get(
                    filter: x => x.StartDate <= toDateTime && x.EndDate >= fromDateTime,
                    includeProperties: "User");

                // Loop through each WorkShiftEnum value to ensure each shift type is represented
                foreach (WorkShiftEnum workShift in Enum.GetValues(typeof(WorkShiftEnum)))
                {
                    // Group the shifts by WorkShift and filter for the current enum value
                    var shiftsForCurrentWorkShift = shifts
                        .Where(s => s.WorkShift == workShift)
                        .ToList();

                    // If no shifts exist for this workShift on the current date, add an empty response
                    if (!shiftsForCurrentWorkShift.Any())
                    {
                        workersInShifts.Add(new WorkerInShiftResponse
                        {
                            workShift = workShift,    // Current WorkShiftEnum value
                            date = date,              // The current date
                            countOfWorker = 0,        // No workers for this shift
                            users = new List<User>()  // Empty user list
                        });
                    }
                    else
                    {
                        // Add a response with actual workers for this shift
                        workersInShifts.Add(new WorkerInShiftResponse
                        {
                            workShift = workShift,                               // Current WorkShiftEnum value
                            date = date,                                         // The current date
                            countOfWorker = shiftsForCurrentWorkShift.Select(s => s.UserId).Distinct().Count(), // Count distinct users
                            users = shiftsForCurrentWorkShift.Select(s => s.User).Distinct().ToList() // List of distinct users in the shift
                        });
                    }
                }
            }

            // Return the list of worker shifts for each day and each shift type
            return workersInShifts;
        }

        public List<User> getAvailableUserForSpecificShift(DateOnly fromDate, DateOnly toDate, WorkShiftEnum workShiftEnum)
        {
            // Step 1: Get all users from the repository
            var allUsers = _unitOfWork.UserRepository.Get(); // Assuming you have a UserRepository to fetch all users

            // Step 2: Get all workers in shift for the given date range
            var workersInShifts = getAllWorkerInShift(fromDate, toDate);

            // Step 3: Create a list to hold available users
            var availableUsers = new List<User>();

            // Step 4: Find users who are not in the shifts for the specified workShiftEnum
            var occupiedUserIds = workersInShifts
                .Where(w => w.workShift == workShiftEnum && w.date >= fromDate && w.date <= toDate)
                .SelectMany(w => w.users.Select(u => u.UserID)) // Assuming User has an Id property
                .Distinct()
                .ToList();

            // Step 5: Filter available users
            availableUsers = allUsers.Where(u => !occupiedUserIds.Contains(u.UserID)).ToList(); // Assuming User has an Id property

            return availableUsers;  
        }


        private bool checkDuplicated(UserShift shift)
        {
            var count = _unitOfWork.UserShiftRepository
                .Get(filter: x => x.UserId == shift.UserId &&
                      x.WorkShift == shift.WorkShift &&
                      (
                        (x.StartDate >= shift.StartDate && x.StartDate <= shift.EndDate)
                        || (x.EndDate >= shift.StartDate && x.EndDate <= shift.EndDate)
                       )
                    )
                .Count();

            return count != 0;
        }

        private bool checkSuitableTime(UserShift shift)
        {
            if(shift.StartDate < DateTime.Today || shift.EndDate < DateTime.Today)
            {
                return false;
            }

            if(shift.StartDate > shift.EndDate)
            {
                return false;
            }

            return true;
        }

        private Guid GetCurrentUserId()
        {
            if (_httpContextAccessor.HttpContext != null && _httpContextAccessor.HttpContext.User != null)
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("nameid");

                if (userIdClaim != null)
                {
                    return Guid.Parse(userIdClaim.Value);
                }
            }

            throw new Exception("User ID not found.");
        }

        
    }
}
