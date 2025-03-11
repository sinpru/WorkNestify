using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Entities.Users;

namespace WorkNestify.DataAccess.Entities.JobApplications
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Resume")]
        public string Resume { get; set; }
        
        [Display(Name = "Cover Letter")]
        public string? CoverLetter { get; set; }

        [Required]
        [Display(Name = "Application Date")]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Modified Date")]
        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Job Seeker")]
        public string JobSeekerId { get; set; }

        [ForeignKey(nameof(JobSeekerId))]
        [Display(Name = "Job Seeker")]
        [BindNever]
        public JobSeeker? JobSeeker { get; set; }

        [Required]
        [Display(Name = "Job")]
        public int JobId { get; set; }

        [ForeignKey(nameof(JobId))]
        [Display(Name = "Job")]
        [BindNever]
        public Job? Job { get; set; }

        [Required]
        [Display(Name = "Job Application Status")]
        public int JobApplicationStatusId { get; set; }

        [ForeignKey(nameof(JobApplicationStatusId))]
        [Display(Name = "Job Application Status")]
        [BindNever]
        public JobApplicationStatus? JobApplicationStatus { get; set; }
    }
}