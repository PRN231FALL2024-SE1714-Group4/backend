using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BOs
{
    public class User : BaseEntity
    {
        [Key]
        public Guid UserID { get; set; } = Guid.NewGuid();
        public Guid RoleID { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Status { get; set; }

        public virtual Role Role { get; set; }
        [JsonIgnore]
        public virtual ICollection<Work> AssignedWorks { get; set; } = new List<Work>();
        [JsonIgnore]
        public virtual ICollection<Work> AssignedByMe { get; set; } = new List<Work>();
    }
}
