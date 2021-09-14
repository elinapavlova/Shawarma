using System.Collections.Generic;

namespace Models.Role
{
    public class Role : BaseModel
    {
        public string Name { get; set; }
        public List<User.User> Users { get; set; }
    }
}