using BOs.DTOS;
using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Request;

namespace Repos
{
    public interface ICageService
    {
        Task<List<Cage>> GetCages();
        Task<Cage> GetCageById(Guid id);
        Task<List<Cage>> GetCagesByAreaId(Guid areaId);
        Task<Cage> CreateCage(CageCreateRequest request);
        Task<bool> UpdateCage(Guid id, CageUpdateRequest request);
        Task<bool> DeleteCage(Guid id);
        List<Animal> GetAnimalsInCage(Guid id);
        IQueryable<Cage> GetCagesOdata();
    }
}
