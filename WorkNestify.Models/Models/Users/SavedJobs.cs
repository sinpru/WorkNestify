using WorkNestify.Models.Models.Jobs;

namespace WorkNestify.Models.Models.Users;

public class SavedJob
{
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }

    public int JobId { get; set; }
    public Job Job { get; set; }

    public DateTime SavedDate { get; set; } = DateTime.UtcNow;
}