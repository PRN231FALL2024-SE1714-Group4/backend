using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs
{
    public class Animal : BaseEntity
    {
        [Key]
        public Guid AnimalID { get; set; } = Guid.NewGuid();
        public string Breed { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Source { get; set; }

        public virtual ICollection<History> Histories { get; set; } = new List<History>();
    }
}
