using System.Collections.Generic;

namespace Models.User
{
    public class User 
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public int IdRole { get; set; }
        
        public Role.Role Role { get; set; }
        
        public List<Order.Order> Orders { get; set; }
    }
}