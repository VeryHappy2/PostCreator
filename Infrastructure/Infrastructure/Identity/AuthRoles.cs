namespace Infrastructure.Identity;

public static class AuthRoles
{
    public const string User = "User";

    public const string Admin = "Admin";
    public static readonly string[] AllRoles = { User, Admin };
}