using System.ComponentModel.DataAnnotations;

namespace BOs.DTOS
{
    public class AreaUpdateRequest
    {
        //[Required]
        //public Guid AreaID { get; set; }

        //[Required]
        [StringLength(100, ErrorMessage = "Area name can't be longer than 100 characters.")]
        public string Name { get; set; }

    }
}
