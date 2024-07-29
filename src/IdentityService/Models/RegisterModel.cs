namespace IdentityServiceAPI.Models
{
    public class RegisterModel (string login, string email, string password)
    {
        public string Login { get; set; } = login;

        public string Email { get; set; } = email;

        public string Password { get; set; } = password;
    }
}
