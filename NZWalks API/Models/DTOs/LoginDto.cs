using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NZWalks_API.Models.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
