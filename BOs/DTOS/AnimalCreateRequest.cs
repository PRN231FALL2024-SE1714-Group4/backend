using System;
using System.ComponentModel.DataAnnotations;
using BOs.Enum;

namespace BOs.DTOS
{
	public class AnimalCreateRequest
    { 
        [Required]
        public AnimalBreed Breed { get; set; }

        [Required]
        public AnimalGender Gender { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string Source { get; set; }
    }
}

