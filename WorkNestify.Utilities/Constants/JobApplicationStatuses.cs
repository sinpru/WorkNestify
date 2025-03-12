namespace WorkNestify.Utilities.Constants;

public static class JobApplicationStatuses
{
    public const string Pending = "Pending";
    public const string Reviewed = "Reviewed";
    public const string InterviewScheduled = "Interview Scheduled";
    public const string Interviewed = "Interviewed";
    public const string Hired = "Hired";
    public const string Rejected = "Rejected";
    public const string Withdrawn = "Withdrawn";

    public static readonly List<string> AllStatuses = new()
    {
        Pending, Reviewed, InterviewScheduled, Interviewed, Hired, Rejected, Withdrawn
    };
}