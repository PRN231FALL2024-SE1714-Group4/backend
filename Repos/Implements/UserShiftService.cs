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

        public List<UserShiftTimeResponse> getAvailableUserForSpecificShift(DateOnly fromDate, DateOnly toDate, WorkShiftEnum workShiftEnum)
        {
            // List to hold available users with their estimated times
            //var availableUsers = new List<UserShiftTimeResponse>();

            //// Loop through each day from 'fromDate' to 'toDate'
            //for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            //{
            //    // Convert the current 'DateOnly' to 'DateTime' for querying
            //    var fromDateTime = date.ToDateTime(new TimeOnly(0, 0));  // Start of the day
            //    var toDateTime = date.ToDateTime(new TimeOnly(23, 59));  // End of the day

            //    // Fetch all shifts for the specific work shift on this date
            //    var userShifts = _unitOfWork.UserShiftRepository.Get(
            //        filter: x => x.WorkShift == workShiftEnum &&
            //                     x.StartDate <= toDateTime &&
            //                     x.EndDate >= fromDateTime,
            //        includeProperties: "User"
            //    ).ToList();

            //    float totalEstimateTime = 0;
            //    // Loop through each shift to calculate the estimate time
            //    foreach (var shift in userShifts)
            //    {
            //        // Initialize total estimate time for this shift

            //        var missionsInShift = new List<WorkMission>();  // To store all missions in this shift
            //        var works = _unitOfWork.WorkRepository.Get(
            //            filter: x => x.Shift = workShiftEnum &&
            //                     x.StartDate <= toDateTime &&
            //                     x.EndDate >= fromDateTime,
            //            );
            //        // Calculate EstimateTime based on missions assigned in this shift
            //        switch (shift.)
            //        {
            //            case WorkMission.FEED:
            //            case WorkMission.CLEAN_CAGE:
            //            case WorkMission.ANIMAL_MOVE:
            //                totalEstimateTime += 0.75f;
            //                missionsInShift.Add(mission);  // Collect missions for response
            //                break;

            //            // You can add more mission cases if necessary
            //            default:
            //                break;
            //        }
            //        // Add the user and their shift information to the available users list
            //        availableUsers.Add(new UserShiftTimeResponse
            //        {
            //            user = shift.User,                 // The user assigned to the shift
            //            workShift = shift.WorkShift,       // The type of shift (morning, evening, etc.)
            //            date = date,                      // The specific date for the shift
            //            EstimateTime = totalEstimateTime,  // The total estimated time for this shift
            //            WorkMissions = missionsInShift     // List of work missions for the shift
            //        });
            //    }
            //}

            //// Return the list of users with their shifts and estimated times for the specific work shift
            //return availableUsers;
            throw new ApplicationException();
        }



        public List<UserShift> getMyShift(DateOnly fromDate, DateOnly toDate)
        {
            // Get the current user's ID
            var currentUserId = GetCurrentUserId();

            // Convert 'fromDate' and 'toDate' to DateTime for comparison
            var fromDateTime = fromDate.ToDateTime(new TimeOnly(0, 0)); // Start of the day
            var toDateTime = toDate.ToDateTime(new TimeOnly(23, 59));   // End of the day

            // Fetch all shifts for the current user within the date range
            var userShifts = _unitOfWork.UserShiftRepository.Get(
                filter: x => x.UserId == currentUserId &&
                             x.StartDate <= toDateTime &&
                             x.EndDate >= fromDateTime
            ).ToList();

            // Return the list of shifts for the current user within the specified date range
            return userShifts;
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
