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
            var availableUsers = new List<UserShiftTimeResponse>();

            // Loop through each day from 'fromDate' to 'toDate'
            for (var date = fromDate; date <= toDate; date = date.AddDays(1))
            {
                // Convert the current 'DateOnly' to 'DateTime' for querying
                var fromDateTime = date.ToDateTime(new TimeOnly(0, 0));  // Start of the day
                var toDateTime = date.ToDateTime(new TimeOnly(23, 59));  // End of the day

                // Fetch all shifts for the specific work shift on this date
                var userShifts = _unitOfWork.UserShiftRepository.Get(
                    filter: x => x.WorkShift == workShiftEnum &&
                                 x.StartDate <= toDateTime &&
                                 x.EndDate >= fromDateTime,
                    includeProperties: "User"
                ).ToList();

                // Loop through each shift to calculate the estimate time for works assigned to the user
                foreach (var shift in userShifts)
                {
                    // Fetch all works assigned to the current user for this shift
                    var works = _unitOfWork.WorkRepository.Get(
                        filter: x => x.Shift == workShiftEnum &&
                                     x.StartDate <= toDateTime &&
                                     x.EndDate >= fromDateTime &&
                                     x.AssigneeID == shift.UserId
                    ).ToList();

                    // If the user has works assigned, calculate the total estimated time
                    if (works.Any())
                    {
                        float totalEstimateTime = 0;

                        // Calculate estimate time based on each work's mission
                        foreach (var work in works)
                        {
                            totalEstimateTime += CalculateEstimateTimeForMission(work.Mission);
                        }

                        // Add the user with the calculated estimate time to the list
                        availableUsers.Add(new UserShiftTimeResponse
                        {
                            user = shift.User,
                            workShift = workShiftEnum,
                            date = date,
                            EstimateTime = totalEstimateTime,
                            WorkMissions = works.Select(w => w.Mission).ToList()  // Add the list of missions (optional)
                        });
                    }
                }
            }

            // Return the list of users with their shifts and estimated times for the specific work shift
            return availableUsers;
        }

        // Helper method to calculate estimate time based on the mission type
        private float CalculateEstimateTimeForMission(WorkMission mission)
        {
            // Define the estimated time for each mission type
            switch (mission)
            {
                case WorkMission.FEED:
                    return 0.5f; // 30 minutes
                case WorkMission.CLEAN_CAGE:
                    return 0.75f; // 45 minutes
                case WorkMission.ANIMAL_MOVE:
                    return 1.0f; // 1 hour
                                 // Add additional cases for other mission types as needed
                default:
                    return 0;
            }
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

        public bool editUserShift(Guid shiftId, UserShiftRequest updatedShiftRequest)
        {
            // Fetch the shift by its ID
            var existingShift = _unitOfWork.UserShiftRepository.GetByID(shiftId);
            if (existingShift == null)
            {
                throw new Exception("Shift not found.");
            }

            // Update the shift details
            existingShift.UserId = updatedShiftRequest.UserId ?? this.GetCurrentUserId();
            existingShift.StartDate = updatedShiftRequest.StartDate.ToDateTime(new TimeOnly(0, 0));
            existingShift.EndDate = updatedShiftRequest.EndDate.ToDateTime(new TimeOnly(23, 59));
            existingShift.WorkShift = updatedShiftRequest.WorkShift;

            // Check if the updated shift has suitable time and no duplicates
            if (!this.checkSuitableTime(existingShift))
            {
                throw new Exception("Invalid time for the shift.");
            }

            if (this.checkDuplicated(existingShift))
            {
                throw new Exception("Shift conflict detected.");
            }

            // Save changes
            _unitOfWork.UserShiftRepository.Update(existingShift);
            _unitOfWork.Save();

            return true;
        }

        public bool deleteUserShift(Guid shiftId)
        {
            // Fetch the shift by its ID
            var existingShift = _unitOfWork.UserShiftRepository.GetByID(shiftId);
            if (existingShift == null)
            {
                throw new Exception("Shift not found.");
            }

            // Delete the shift
            _unitOfWork.UserShiftRepository.Delete(existingShift);
            _unitOfWork.Save();

            return true;
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

        public List<UserShift> getAllUserShifts()
        {
            throw new NotImplementedException();
        }

        public UserShift getUserShiftById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
