using System.ComponentModel.DataAnnotations;

namespace NZWalks_API.Models.DTOs
{
    public class AddRegionRequestDto
    {
        [Required]
        [Length(3, 3, ErrorMessage = "{0} has to be {1} characters only")]
        public string Code { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage ="{0} has to be a maximum of {1} characters")]
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
