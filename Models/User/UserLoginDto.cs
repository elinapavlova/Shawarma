using System.ComponentModel.DataAnnotations;

namespace Models.User
{
    public class UserLoginDto
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}