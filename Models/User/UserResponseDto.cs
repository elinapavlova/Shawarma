using System.Collections.Generic;
using Models.Role;

namespace Models.User
{
    public class UserResponseDto
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public int IdRole { get; set; }
        
        
       // public List<Order.OrderResponseDto> Orders { get; set; }
    }
}