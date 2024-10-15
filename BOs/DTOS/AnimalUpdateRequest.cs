using BOs.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs.DTOS
{
    public class AnimalUpdateRequest
    {
        public AnimalBreed? Breed { get; set; }
        public AnimalGender? Gender { get; set; }
        public int? Age { get; set; }
        public string? Source { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
