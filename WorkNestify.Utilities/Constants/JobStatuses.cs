namespace WorkNestify.Utilities.Constants;

public static class JobStatuses
{
    public const string Open = "Open";
    public const string Closed = "Closed";
    public const string Pending = "Pending";
    public const string Declined = "Declined";
    public const string Expired = "Expired";

    public static readonly List<string> AllStatuses = new()
    {
        Open, Closed, Declined, Pending, Expired
    };
}