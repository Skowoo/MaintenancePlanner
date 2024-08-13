namespace IdentityServiceAPI.Models
{
    public readonly struct RegisterModel(string login, string email, string password)
    {
        public string Login { get; init; } = login;

        public string Email { get; init; } = email;

        public string Password { get; init; } = password;
    }
}
