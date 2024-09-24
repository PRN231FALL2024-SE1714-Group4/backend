using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs
{
    public class History : BaseEntity
    {
        [Key]
        public Guid HistoryID { get; set; } = Guid.NewGuid();

        [Required]
        public Guid AnimalID { get; set; }
        [Required]
        public Guid CageID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }

        public virtual Animal Animal { get; set; }
        public virtual Cage Cage { get; set; }
    }
}
