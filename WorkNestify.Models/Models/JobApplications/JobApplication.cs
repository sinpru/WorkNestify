using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Models.Models.Users;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Models.Models.JobApplications
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Resume")]
        public string? Resume { get; set; }
        
        [Display(Name = "Cover Letter")]
        public string? CoverLetter { get; set; }
        
        [Display(Name = "Application Date")]
        public DateTime? ApplicationDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Job Seeker")]
        public string ApplicationUserId { get; set; }

        [ForeignKey(nameof(ApplicationUserId))]
        [Display(Name = "Job Seeker")]
        [BindNever]
        public ApplicationUser? ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Job")]
        public int JobId { get; set; }

        [ForeignKey(nameof(JobId))]
        [Display(Name = "Job")]
        [BindNever]
        public Job? Job { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } = JobApplicationStatuses.Pending;
    }
}