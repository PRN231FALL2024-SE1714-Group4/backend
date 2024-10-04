using BOs.DTOS;
using BOs;
using DAOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BOs.Enum;

namespace Repos.Implements
{
    public class WorkService : IWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WorkService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        // 1. Function to create Work
        public void CreateWork(CreateWorkRequest request)
        {
            var currentUserId = GetCurrentUserId();

            // Create new Work
            var work = new Work
            {
                RoleID = request.RoleID,
                AreaID = request.AreaID,
                AssignerID = currentUserId, // Set the current user as the assigner
                AssigneeID = request.AssigneeID,
                Status = WorkStatus.OPEN,
                Shift = request.Shift,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            _unitOfWork.WorkRepository.Insert(work);
            _unitOfWork.Save();
        }

        // 2. Function for Assignee to view their Tasks
        public IEnumerable<Work> ViewMyWork()
        {
            var currentUserId = GetCurrentUserId();
            return _unitOfWork.WorkRepository.Get(filter: w => w.AssigneeID == currentUserId).ToList();
        }

        // 4. Function for Assigner to observe all assigned tasks
        public IEnumerable<Work> ViewAssignedTasks()
        {
            var currentUserId = GetCurrentUserId();
            return _unitOfWork.WorkRepository.Get(filter: w => w.AssignerID == currentUserId).ToList();
        }

        // Method to get the current user's ID
        private Guid GetCurrentUserId()
        {
            var userClaims = _httpContextAccessor.HttpContext?.User;
            if (userClaims != null && userClaims.Identity.IsAuthenticated)
            {
                var userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    return Guid.Parse(userIdClaim.Value);
                }
            }
            throw new UnauthorizedAccessException("User is not authenticated.");
        }
    }
}
