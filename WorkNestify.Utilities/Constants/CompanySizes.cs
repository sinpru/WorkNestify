namespace WorkNestify.Utilities.Constants;

public static class CompanySizes
{
    public const string Small = "Small";
    public const string Medium = "Medium";
    public const string Large = "Large";

    public static readonly List<string> AllSizes = new()
    {
        Small, Medium, Large
    };
}