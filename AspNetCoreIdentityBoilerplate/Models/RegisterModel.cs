using System.ComponentModel.DataAnnotations;

namespace AspNetCoreIdentityBoilerplate.Models
{
    public class RegisterModel
    {
        [Required]
        [MaxLength(255)]
        public string Login { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
