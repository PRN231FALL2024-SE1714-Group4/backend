using BOs;
using BOs.DTOS;
using BOs.Enum;
using DAOs;
using Microsoft.AspNetCore.Http;
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

        public WorkResponse CreateWork(WorkCreateRequest request)
        {
            var currentUserId = this.GetCurrentUserId();
            var assigner = _unitOfWork.UserRepository.GetByID(currentUserId);
            var assignee = _unitOfWork.UserRepository.GetByID(request.AssigneeID);
            var assigneeID = assignee != null ? assignee.UserID : throw new Exception("There are no assignee");

            // Create new Work
            var work = new Work
            {
                //RoleID = assigner.RoleID,
                //AreaID = request.AreaID,
                CageID = request.CageId,
                AssignerID = currentUserId, // Set the current user as the assigner
                AssigneeID = assigneeID,
                Status = WorkStatus.OPEN,
                Shift = request.Shift,
                Description = request.Description,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Mission = request.Mission
            };

            _unitOfWork.WorkRepository.Insert(work);
            _unitOfWork.Save();

            var createdWork = this.GetWorkByID(work.WorkId);
            return MapWorkToWorkResponse(createdWork);
        }

        // 2. Function for Assignee to view their Tasks
        public IEnumerable<WorkResponse> ViewMyWork()
        {
            var currentUserId = GetCurrentUserId();
            var works = _unitOfWork.WorkRepository.Get(
                filter: w => w.AssigneeID == currentUserId,
                includeProperties: "Assigner,Assignee,Cage").ToList();

            return works.Select(w => MapWorkToWorkResponse(w)).ToList();
        }


        // 4. Function for Assigner to observe all assigned tasks
        public IEnumerable<WorkResponse> ViewAssignedTasks()
        {
            var currentUserId = GetCurrentUserId();
            var assignedTasks = _unitOfWork.WorkRepository.Get(
                filter: w => w.AssignerID == currentUserId,
                includeProperties: "Assigner,Assignee,Cage").ToList();

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
                .Get(filter: w => w.WorkId == id, includeProperties: "Cage,Assigner,Assignee")
                .FirstOrDefault();

            return MapWorkToWorkResponse(work);
        }

        private WorkResponse MapWorkToWorkResponse(Work work)
        {
            // Assuming completedTask and totalTask can be calculated somehow
            //int completedTasks = _unitOfWork.ReportRepository.Get(filter: r => r.Status == ReportStatus.DONE && r.WorkId == work.WorkId).Count();
            //int totalTasks = _unitOfWork.ReportRepository.Get(filter: r => r.WorkId == work.WorkId).ToList().Count();
            Area area = _unitOfWork.AreaRepository.GetByID(work.Cage.AreaID);
            return new WorkResponse
            {
                WorkId = work.WorkId,
                //RoleID = work.RoleID,
                //AreaID = work.AreaID,
                CageID = work.CageID,
                AssignerID = work.AssignerID,
                AssigneeID = work.AssigneeID,
                Status = work.Status,
                Shift = work.Shift,
                Description = work.Description,
                StartDate = work.StartDate,
                EndDate = work.EndDate,
                //compltedTask = completedTasks,
                //totalTask = totalTasks,
                Cage = work.Cage,
                Assignee = work.Assignee,
                Assigner = work.Assigner,
                Area = area,
                Mission = work.Mission
            };


        }

        public List<WorkResponse> GetWorks()
        {
            var works = _unitOfWork.WorkRepository.Get(includeProperties: "Cage,Assigner,Assignee").ToList();
            return works.Select(w => MapWorkToWorkResponse(w)).ToList();
        }

        public WorkResponse UpdateWork(Guid id, WorkUpdateRequest request)
        {
            // Find the work by ID
            var work = _unitOfWork.WorkRepository.GetByID(id);

            if (work == null)
            {
                throw new Exception("Work not found.");
            }

            // Update the properties of the work with null-checking
            work.Description = !string.IsNullOrEmpty(request.Description) ? request.Description : work.Description;
            work.StartDate = request.StartDate ?? work.StartDate;
            work.EndDate = request.EndDate ?? work.EndDate;
            work.Shift = request.Shift ?? work.Shift;
            work.AssigneeID = request.AssigneeID ?? work.AssigneeID;
            work.Mission = request.Mission ?? work.Mission;
            work.Status = request.Status ?? work.Status;

            // Save the updated work
            _unitOfWork.WorkRepository.Update(work);
            _unitOfWork.Save();

            // Return the updated WorkResponse
            return MapWorkToWorkResponse(_unitOfWork.WorkRepository.Get(includeProperties: "Cage,Assigner,Assignee").FirstOrDefault());
        }

        public bool DeleteWork(Guid id)
        {
            // Find the work by ID
            var work = _unitOfWork.WorkRepository.GetByID(id);

            if (work == null)
            {
                throw new Exception("Work not found.");
            }

            // Remove the work
            _unitOfWork.WorkRepository.Delete(work);
            _unitOfWork.Save();

            return true;
        }

    }
}
