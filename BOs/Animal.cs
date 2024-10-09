using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BOs.Enum;

namespace BOs
{
    public class Animal : BaseEntity
    {
        [Key]
        public Guid AnimalID { get; set; } = Guid.NewGuid();
        public AnimalBreed Breed { get; set; }
        public AnimalGender Gender { get; set; }
        public int Age { get; set; }
        public string Source { get; set; }
        public DateTime? DateOfBirth { get; set; }   


        [JsonIgnore]        
        public virtual ICollection<History> Histories { get; set; } = new List<History>();
    }
}
