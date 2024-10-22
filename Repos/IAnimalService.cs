using BOs.DTOS;
using BOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface IAnimalService
    {
        List<Animal> GetAnimals();
        Task<Animal> GetAnimalById(Guid id);
        Task<Animal> CreateAnimal(AnimalCreateRequest request);
        Task<Animal> UpdateAnimal(Guid id, AnimalUpdateRequest request);
        Task<bool> DeleteAnimal(Guid id);
        public Cage getCurrentCage(Guid id);
    }
}
