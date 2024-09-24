﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOs
{
    public class Cage : BaseEntity
    {
        [Key]
        public Guid CageID { get; set; } = Guid.NewGuid();
        public Guid AreaID { get; set; }

        public virtual Area Area { get; set; }
        public virtual ICollection<History> Histories { get; set; } = new List<History>();
    }
}
