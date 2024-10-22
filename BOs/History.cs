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
        public Guid AnimalID { get; set; }
        public Guid CageID { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        // Nullable FromDate and ToDate fields
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public virtual Animal Animal { get; set; }
        public virtual Cage Cage { get; set; }
    }
}
