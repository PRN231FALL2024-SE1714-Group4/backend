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
using Repos.Response;

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

        public WorkResponse CreateWork(CreateWorkRequest request)
        {
            var assigner = _unitOfWork.UserRepository.GetByID(request.AssigerID);
            var currentUserId = assigner != null ? assigner.UserID : throw new Exception("There are no assigner");
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

            var createdWork = _unitOfWork.WorkRepository.GetByID(work.WorkId);
            return MapWorkToWorkResponse(createdWork);
        }

        // 2. Function for Assignee to view their Tasks
        public IEnumerable<WorkResponse> ViewMyWork()
        {
            var currentUserId = GetCurrentUserId();
            var works = _unitOfWork.WorkRepository.Get(
                filter: w => w.AssigneeID == currentUserId,
                includeProperties: "Assigner,Assignee,Area").ToList();

            return works.Select(w => MapWorkToWorkResponse(w)).ToList();
        }


        // 4. Function for Assigner to observe all assigned tasks
        public IEnumerable<WorkResponse> ViewAssignedTasks()
        {
            var currentUserId = GetCurrentUserId();
            var assignedTasks = _unitOfWork.WorkRepository.Get(
                filter: w => w.AssignerID == currentUserId,
                includeProperties: "Assigner,Assignee,Area").ToList();

            return assignedTasks.Select(w => MapWorkToWorkResponse(w)).ToList();
        }


        // Method to get the current user's ID
        public Guid GetCurrentUserId()
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

        // Method to get a Work by ID and return WorkResponse
        public WorkResponse GetWorkByID(Guid id)
        {
            var work = _unitOfWork.WorkRepository
                .Get(filter: w => w.WorkId == id, includeProperties: "Area,Assigner,Assignee")
                .FirstOrDefault();

            return MapWorkToWorkResponse(work);
        }

        private WorkResponse MapWorkToWorkResponse(Work work)
        {
            // Assuming completedTask and totalTask can be calculated somehow
            int completedTasks = _unitOfWork.ReportRepository.Get(filter: r => r.Status == ReportStatus.DONE && r.WorkId == work.WorkId).Count();
            int totalTasks = _unitOfWork.ReportRepository.Get(filter: r => r.WorkId == work.WorkId).ToList().Count();

            return new WorkResponse
            {
                WorkId = work.WorkId,
                RoleID = work.RoleID,
                AreaID = work.AreaID,
                AssignerID = work.AssignerID,
                AssigneeID = work.AssigneeID,
                Status = work.Status,
                Shift = work.Shift,
                Description = work.Description,
                StartDate = work.StartDate,
                EndDate = work.EndDate,
                compltedTask = completedTasks,
                totalTask = totalTasks
            };
        }
    }
}
