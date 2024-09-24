namespace IdentityServiceAPI.Models
{
    public readonly struct RoleAssignChangeModel(string login, string rolename)
    {
        public string Login { get; init; } = login;

        public string RoleName { get; init; } = rolename;
    }
}
