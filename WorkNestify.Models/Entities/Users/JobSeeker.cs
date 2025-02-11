using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkNestify.Models.Entities.JobApplications;

namespace WorkNestify.Models.Entities.Users;

public class JobSeeker : ApplicationUser
{
    [Required]
    [Column(TypeName = "NVARCHAR(255)")]
    public string ResumeUrl { get; set; } = string.Empty;
    
    public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}