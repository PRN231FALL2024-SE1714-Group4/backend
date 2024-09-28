using Azure.Core;
using BOs;
using BOs.DTOS;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos.Implements
{
    public class AreaService : IAreaService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AreaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public List<Area> GetAreas()
        {
            return _unitOfWork.AreaRepository.Get().ToList();
        }

        public Area GetArea(Guid id)
        {
            return _unitOfWork.AreaRepository.Get(
                filter: p => p.AreaID == id
            ).FirstOrDefault();
        }

        public Area CreateArea(AreaCreateRequest areaCreateRequest)
        {
            var area = new Area
            {
                Name = areaCreateRequest.Name
            };

            _unitOfWork.AreaRepository.Insert(area);
            _unitOfWork.Save();

            return this.GetArea(area.AreaID);
        }

        public Area UpdateArea(Guid id, AreaUpdateRequest request)
        {
            var areaToUpdate = _unitOfWork.AreaRepository.GetByID(id);
            if (areaToUpdate == null)
            {
                throw new Exception("Area not found");
            }

            areaToUpdate.Name = request.Name;
            _unitOfWork.AreaRepository.Update(id, areaToUpdate);
            _unitOfWork.Save();

            return GetArea(id);
        }

        public Boolean DeleteArea(Guid id)
        {
            var isDeleted = _unitOfWork.AreaRepository.Delete(id);
            _unitOfWork.Save();
            return isDeleted;
            throw new NotImplementedException();
        }
    }
}
