using System.ComponentModel.DataAnnotations;

namespace Models.User
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        
        public string Password { get; set; }

    }
}