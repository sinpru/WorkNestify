namespace WorkNestify.Utilities.Constants;

public static class Roles
{
    public const string Admin = "Admin";
    public const string Employer = "Employer";
    public const string JobSeeker = "JobSeeker";

    public static readonly List<string> AllRoles = new()
    {
        Admin, Employer, JobSeeker
    };
}