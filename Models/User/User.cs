using System.Collections.Generic;
using Models.Role;

namespace Models.User
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public int IdRole { get; set; }

        public RolesEnum Role { get; set; }
        public List<Order.Order> Orders { get; set; }
    }
}