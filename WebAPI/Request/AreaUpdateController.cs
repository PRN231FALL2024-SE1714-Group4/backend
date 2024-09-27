using System.ComponentModel.DataAnnotations;

namespace WebAPI.Request
{
    public class AreaUpdateController
    {
        [Required]
        public Guid AreaID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Area name can't be longer than 100 characters.")]
        public string Name { get; set; }

    }
}
