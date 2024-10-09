﻿using BOs.DTOS;
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
    public class AnimalService : IAnimalService
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
                .Get(filter: a => a.AnimalID == id)
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
            return _unitOfWork.AnimalRepository
                .Get(filter: a => a.AnimalID == animal.AnimalID)
                .FirstOrDefault();
        }

        public async Task<Animal> UpdateAnimal(Guid id, AnimalUpdateRequest request)
        {
            // Retrieve the existing animal by ID
            var existingAnimal = _unitOfWork.AnimalRepository
                .Get(filter: a => a.AnimalID == id)
                .FirstOrDefault();

            if (existingAnimal == null)
            {
                // Handle case where the animal is not found
                throw new Exception("Animal not found.");
            }

            // Update the properties of the existing animal
            existingAnimal.Breed = request.Breed ?? existingAnimal.Breed;
            existingAnimal.Gender = request.Gender ?? existingAnimal.Gender;
            existingAnimal.Age = request.Age ?? existingAnimal.Age;
            existingAnimal.Source = request.Source ?? existingAnimal.Source;

            // Update the animal in the repository
            _unitOfWork.AnimalRepository.Update(existingAnimal);
            _unitOfWork.Save(); // Save changes asynchronously

            // Return the updated animal
            return _unitOfWork.AnimalRepository
                .Get(filter: a => a.AnimalID == existingAnimal.AnimalID)
                .FirstOrDefault();
        }
    }
}

