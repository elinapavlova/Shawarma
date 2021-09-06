using System;
using System.Collections.Generic;

namespace Models.User
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public int IdRole { get; set; }
        
        
        public List<Order.OrderResponseDto> Orders { get; set; }
    }
}