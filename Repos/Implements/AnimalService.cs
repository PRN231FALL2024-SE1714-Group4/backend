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
	public class AnimalService
	{
        private readonly IUnitOfWork _unitOfWork;
        public AnimalService(IUnitOfWork unitOfWork)
		{
            _unitOfWork = unitOfWork;
        }

        public List<Animal> GetAnimals()
        {
            return _unitOfWork.AnimalRepository.Get().ToList();
        }
        public async Task<Animal> GetAnimalById(Guid id)
        {
            return _unitOfWork.AnimalRepository
                .Get(filter: a => a.AnimalID == id, includeProperties: "Area,Histories")
                .FirstOrDefault();
        }

        public async Task<Animal> CreateAnimal(AnimalCreateRequest request)
        {
            var animal = new Animal
            {
                Breed = request.Breed,
                Gender = request.Gender,
                Age = request.Age,
                Source = request.Source
            };

            _unitOfWork.AnimalRepository.Insert(animal);
            _unitOfWork.Save();
            return animal;
        }

    }
}

