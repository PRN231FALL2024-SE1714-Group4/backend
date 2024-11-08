﻿using BOs;
using BOs.DTOS;
using BOs.Enum;
using DAOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Repos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Implements
{
    public class HealthReportService : IHealthReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HealthReportService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        List<BOs.HealthReport> IHealthReportService.GetAllHealthReports()
        {
            return _unitOfWork.HealthReportRepository.Get().ToList();
        }

        BOs.HealthReport IHealthReportService.GetHealthReportById(Guid id)
        {
            return _unitOfWork.HealthReportRepository
                .Get(filter: x => x.HelthReportID == id,
                    includeProperties: "Cage,User")
                .FirstOrDefault();
        }

        public void AddHealthReport(HealthReportCreateRequest healthReportCreate)
        {
            if (healthReportCreate == null)
                throw new ArgumentNullException(nameof(healthReportCreate));

            var healthReport = new BOs.HealthReport()
            {
                CageID = healthReportCreate.CageID,
                Status = healthReportCreate.Status,
                DateTime = healthReportCreate.DateTime,
                Description = healthReportCreate.Description,
                //WorkShift = healthReportCreate.WorkShift,
                UserID = this.GetCurrentUserId(),
            };

            _unitOfWork.HealthReportRepository.Insert(healthReport);
            _unitOfWork.Save();
        }

        public bool UpdateHealthReport(Guid id, HealthReportUpdateRequest healthReportUpdate)
        {
            if (healthReportUpdate == null)
                throw new ArgumentNullException(nameof(healthReportUpdate));

            var existingHealthReport = _unitOfWork.HealthReportRepository.GetByID(id);

            existingHealthReport.Description = healthReportUpdate.Description ?? existingHealthReport.Description;
            existingHealthReport.DateTime = healthReportUpdate.DateTime ?? existingHealthReport.DateTime;
            existingHealthReport.Status = healthReportUpdate?.Status ?? existingHealthReport.Status;
            //existingHealthReport.WorkShift = healthReportUpdate?.WorkShift ?? existingHealthReport.WorkShift;


            bool isUpdated = _unitOfWork.HealthReportRepository.Update(id, existingHealthReport);
            if (isUpdated)
            {
                _unitOfWork.Save();
            }
            return isUpdated;
        }

        public bool DeleteHealthReport(Guid id)
        {
            bool isDeleted = _unitOfWork.HealthReportRepository.Delete(id);
            if (isDeleted)
            {
                _unitOfWork.Save();
            }
            return isDeleted;
        }

        public List<CageNeedToReport> GetCageNeedToReport()
        {
            Guid currentUserId = this.GetCurrentUserId();
            List<Work> works = _unitOfWork.WorkRepository
                .Get(filter: x => x.AssigneeID == currentUserId)
                .ToList();

            var groupedWorks = works.GroupBy(
                    x => new { x.CageID, x.StartDate.Date },  // Group by CageID and StartDate (ignoring time part)
                    (key, g) => new { CageID = key.CageID, StartDate = key.Date, Works = g.ToList() })
                .ToList();

            List<CageNeedToReport> cagesToReport = new List<CageNeedToReport>();

            foreach (var group in groupedWorks)
            {
                var healthReport = _unitOfWork.HealthReportRepository
                    .Get(filter: x => x.CageID == group.CageID && x.DateTime == group.StartDate.Date)
                    .ToList();
                // Check if all works in the group have status DONE
                if (group.Works.All(work => work.Status == WorkStatus.DONE) && healthReport.Count == 0)
                {
                    // Add the Cage to the report list if all works are DONE
                    var cage = _unitOfWork.CageRepository.GetByID(group.CageID);
                    if (cage != null)
                    {
                        cagesToReport.Add(new CageNeedToReport()
                        {
                            Cage = cage,
                            DateTime = group.StartDate.Date,
                        });
                    }
                }
            }
            return cagesToReport;

            throw new NotImplementedException();
        }



        public List<BOs.HealthReport> GetHealthEeportByAnimal(Guid animalID)
        {
            var historyByAnimal = _unitOfWork.HistoryRepository
                .Get(filter: x => x.AnimalID == animalID,
                includeProperties: "Animal,Cage");

            List<BOs.HealthReport> healthReportByAnimal = new List<BOs.HealthReport>();
            foreach(var history in historyByAnimal)
            {
                var fromDate = history.FromDate?.Date;
                var toDate = history.ToDate?.Date;

                var healthReport = _unitOfWork.HealthReportRepository
                    .Get(filter: x => x.DateTime.Date >= fromDate
                                      && (toDate == null || x.DateTime.Date <= toDate)
                                      && x.CageID == history.CageID,
                         includeProperties: "Cage")
                    .ToList();
                healthReportByAnimal.AddRange(healthReport);
            }
            return healthReportByAnimal;
            throw new NotImplementedException();
        }

        public List<BOs.HealthReport> GetMyHealthReport()
        {
            Guid currentUserId = this.GetCurrentUserId();
            var myHealthReport = _unitOfWork.HealthReportRepository
                .Get(filter: x => x.UserID == currentUserId,
                    includeProperties: "Cage")
                .ToList();

            return myHealthReport;
            throw new NotImplementedException();
        }

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
    }
}
