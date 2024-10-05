using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BOs
{
    public class Area : BaseEntity
    {
        [Key]
        public Guid AreaID { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<Cage> Cages { get; set; } = new List<Cage>();
        [JsonIgnore]
        public virtual ICollection<Work> Works { get; set; } = new List<Work>();
    }
}
