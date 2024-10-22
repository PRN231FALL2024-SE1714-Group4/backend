using BOs.DTOS;
using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAOs;
using Microsoft.IdentityModel.Tokens;

namespace Repos.Implements
{
    public class HistoryService : IHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<History>> GetAllHistoriesAsync()
        {
            return _unitOfWork.HistoryRepository.Get(includeProperties: "Cage,Animal").ToList();
        }

        public async Task<History> GetHistoryByIdAsync(Guid historyId)
        {
            return _unitOfWork.HistoryRepository
                .Get(   filter: x => x.HistoryID == historyId,
                        includeProperties: "Cage,Animal")
                .FirstOrDefault();
        }

        public async Task<History> CreateHistoryAsync(HistoryCreateRequest request)
        {
            var animalExists = _unitOfWork.AnimalRepository.Get(filter: a => a.AnimalID == request.AnimalID);
            if (animalExists == null)
            {
                // Handle the case where the AnimalID doesn't exist
                throw new Exception("The specified AnimalID does not exist.");
            }
            var cageExists = _unitOfWork.CageRepository.Get(filter: a => a.CageID == request.CageID);
            if (cageExists == null)
            {
                // Handle the case where the AnimalID doesn't exist
                throw new Exception("The specified CageID does not exist.");
            }

            var currentCageOfAnimal = _unitOfWork.HistoryRepository
                .Get(filter: x => x.AnimalID == request.AnimalID &&
                    (x.ToDate == null || x.ToDate >= DateTime.UtcNow))
                .ToList();

            if(currentCageOfAnimal.Count != 0)
            {
                throw new Exception("Animal was in another Cage.");
            }

            var newHistory = new History
            {
                HistoryID = Guid.NewGuid(),
                AnimalID = request.AnimalID,
                CageID = request.CageID,
                Description = request.Description,
                Status = "",
                FromDate = request.FromDate,
                ToDate = request.ToDate
            };

            if(!this.checkDuplicated(newHistory))
            {
                throw new Exception("Duplicated History");
            }

            _unitOfWork.HistoryRepository.Insert(newHistory);
            _unitOfWork.Save();

            return newHistory;
        }   

        public async Task<History> UpdateHistoryAsync(Guid historyId, HistoryUpdateRequest request)
        {
            var existingHistory = _unitOfWork.HistoryRepository
                .Get(filter: x => x.HistoryID == historyId)
                .FirstOrDefault();

            if (existingHistory == null)
            {
                return null; // or throw an exception, based on your needs
            }

            // Update the fields
            existingHistory.AnimalID = request.AnimalID ?? existingHistory.AnimalID;
            existingHistory.CageID = request.CageID ?? existingHistory.CageID;
            existingHistory.Description = request.Description ?? existingHistory.Description;
            //existingHistory.Status = request.Status ?? existingHistory.Status;
            existingHistory.FromDate = request.FromDate ?? existingHistory.FromDate;
            existingHistory.ToDate = request.ToDate ?? existingHistory.ToDate;

            _unitOfWork.HistoryRepository.Update(existingHistory);
            _unitOfWork.Save();

            return existingHistory;
            //throw new Exception();
        }

        public async Task<bool> DeleteHistoryAsync(Guid historyId)
        {
            var history = _unitOfWork.HistoryRepository
                .Get(filter: x => x.HistoryID == historyId)
                .FirstOrDefault();

            if (history == null)
            {
                return false;
            }

            _unitOfWork.HistoryRepository.Delete(history);
            _unitOfWork.Save();
            return true;
        }

        private bool checkDuplicated(History history)
        {
            var existingHistory = _unitOfWork.HistoryRepository
                .Get(filter: x => x.CageID == history.CageID &&
                                x.AnimalID == history.AnimalID &&
                                x.FromDate == history.FromDate)
                .ToList();

            if(existingHistory == null)
            {
                return false;
            }

            return true;
        }
    }
}
