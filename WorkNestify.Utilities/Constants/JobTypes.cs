namespace WorkNestify.Utilities.Constants;

public static class JobTypes
{
    public const string FullTime = "Full-Time";
    public const string PartTime = "Part-Time";
    public const string Remote = "Remote";
    public const string Freelance = "Freelance";

    public static readonly List<string> AllTypes = new()
    {
        FullTime, PartTime, Remote, Freelance
    };
}