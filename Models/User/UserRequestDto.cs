namespace Models.User
{
    public class UserRequestDto : BaseModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public int IdRole { get; set; }
    }
}