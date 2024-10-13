namespace JwtAuthenticationApp.Models
{
    public readonly struct LoginModel(string login, string password)
    {
        public string Login { get; init; } = login;

        public string Password { get; init; } = password;
    }
}
