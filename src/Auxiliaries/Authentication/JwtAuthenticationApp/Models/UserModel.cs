namespace JwtAuthenticationApp.Models
{
    public readonly struct UserModel(string id, string login, string[] roles)
    {
        public string Id { get; init; } = id;

        public string Login { get; init; } = login;

        public string[] Roles { get; init; } = roles;
    }
}
