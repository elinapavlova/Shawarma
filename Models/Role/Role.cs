using System.Collections.Generic;

namespace Models.Role
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public List<User.User> Users { get; set; }
    }
}