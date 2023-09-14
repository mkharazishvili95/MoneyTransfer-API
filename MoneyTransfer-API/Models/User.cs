namespace MoneyTransfer_API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public double Balance { get; set; }
        public bool IsBlocked { get; set; }
        public User()
        {
            Role = Roles.User;
            Balance = 0;
            IsBlocked = false;
        }
    }
}
