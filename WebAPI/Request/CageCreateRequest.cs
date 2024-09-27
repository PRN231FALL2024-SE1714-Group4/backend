using System.ComponentModel.DataAnnotations;

namespace WebAPI.Request
{
    public class CageCreateRequest
    {
        [Required]
        public string CageName { get; set; }

        [Required]
        public Guid AreaID { get; set; }
    }
}
