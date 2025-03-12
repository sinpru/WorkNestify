using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using WorkNestify.Models.Models.Companies;
using WorkNestify.Models.Models.JobApplications;

namespace WorkNestify.Models.Models.Users;

public class ApplicationUser : IdentityUser
{
    [Required]
    [Column(TypeName = "NVARCHAR(100)")]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [Display(Name = "Company")]
    public int? CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    [Display(Name = "Company")]
    public Company? Company { get; set; } = null!;
    
    [Display(Name = "Resume")]
    public string ResumeUrl { get; set; } = string.Empty;
    
    public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
}