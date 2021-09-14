using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Models.User
{
    public class User : BaseModel
    {
        public string Email { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string UserName { get; set; }
        public int IdRole { get; set; }
        
        public Role.Role Role { get; set; }
        
        public List<Order.Order> Orders { get; set; }
    }
}