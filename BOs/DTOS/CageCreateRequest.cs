using System.ComponentModel.DataAnnotations;

namespace BOs.DTOS
{
    public class CageCreateRequest
    {
        [Required]
        public string CageName { get; set; }

        [Required]
        public Guid AreaID { get; set; }
    }
}
