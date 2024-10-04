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
        public Work CreateWork(CreateWorkRequest request)
        {
            var assigner = _unitOfWork.UserRepository.GetByID(request.AssigerID);
            var currentUserId = assigner != null? assigner.UserID : throw new Exception("There are no assigner");
            var assignee = _unitOfWork.UserRepository.GetByID(request.AssigneeID);
            var assigneeID = assignee != null ? assignee.UserID : throw new Exception("There are no assignee");

            // Create new Work
            var work = new Work
            {
                RoleID = assigner.RoleID,
                AreaID = request.AreaID,
                AssignerID = currentUserId, // Set the current user as the assigner
                AssigneeID = assigneeID,
                Status = WorkStatus.OPEN,
                Shift = request.Shift,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };

            _unitOfWork.WorkRepository.Insert(work);
            _unitOfWork.Save();

            return _unitOfWork.WorkRepository.GetByID(work.WorkId);
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

        public Work GetWorkByID(Guid id)
        {
            return _unitOfWork.WorkRepository
                .Get(filter: w => w.WorkId == id, includeProperties: "Area,Assigner,Assignee")
                .FirstOrDefault();
        }
    }
}
