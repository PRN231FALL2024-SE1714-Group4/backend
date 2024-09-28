using BOs.DTOS;
using BOs;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Request;

namespace Repos.Implements
{
    public class CageService : ICageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Cage>> GetCages()
        {
            return _unitOfWork.CageRepository.Get(includeProperties: "Area").ToList();
        }

        public async Task<Cage> GetCageById(Guid id)
        {
            return _unitOfWork.CageRepository
                .Get(filter: c => c.CageID == id, includeProperties: "Area,Histories")
                .FirstOrDefault();
        }

        public async Task<List<Cage>> GetCagesByAreaId(Guid areaId)
        {
            return _unitOfWork.CageRepository
                .Get(filter: c => c.AreaID == areaId, includeProperties: "Area")
                .ToList();
        }

        public async Task<Cage> CreateCage(CageCreateRequest request)
        {
            var cage = new Cage
            {
                CageName = request.CageName,
                AreaID = request.AreaID
            };

            _unitOfWork.CageRepository.Insert(cage);
            _unitOfWork.Save();
            return cage;
        }

        public async Task<bool> UpdateCage(Guid id, CageUpdateRequest request)
        {
            var cageToUpdate = _unitOfWork.CageRepository.GetByID(id);
            if (cageToUpdate == null) return false;

            cageToUpdate.AreaID = request.AreaID;

            _unitOfWork.CageRepository.Update(id, cageToUpdate);
            _unitOfWork.Save();
            return true;
        }

        public async Task<bool> DeleteCage(Guid id)
        {
            var isDeleted = _unitOfWork.CageRepository.Delete(id);
            _unitOfWork.Save();
            return isDeleted;
        }
    }
}
