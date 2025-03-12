namespace WorkNestify.Utilities.Constants;

public static class JobLevels
{
    public const string Intern = "Intern";
    public const string Fresher = "Fresher";
    public const string Junior = "Junior";
    public const string Senior = "Senior";

    public static readonly List<string> AllLevels = new()
    {
        Intern, Fresher, Junior, Senior
    };
}