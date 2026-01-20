using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WorkNestify.Models.Models.Companies;
using WorkNestify.Models.Models.JobApplications;
using WorkNestify.Models.Models.Users;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Models.Models.Jobs
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Job Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "Salary")]
        public double Salary { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        
        [Display(Name = "Job Type")]
        public string Type { get; set; } = JobTypes.FullTime;
        
        [Display(Name = "Job Status")]
        public string Status { get; set; } = JobStatuses.Pending;
        
        [Display(Name = "Job Level")]
        public string Level { get; set; } = JobLevels.Fresher;

        [Required]
        [Display(Name = "Job Category")]
        public int JobCategoryId { get; set; }

        [ForeignKey(nameof(JobCategoryId))]
        [BindNever]
        [Display(Name = "Job Category")]
        public JobCategory? JobCategory { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [BindNever]
        [Display(Name = "Company")]
        public Company? Company { get; set; }
        
        [Required]
        [Display(Name = "Province")]
        public int ProvinceCode { get; set; }
        
        [Required]
        [Display(Name = "Ward")]
        public int WardCode { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
        public ICollection<SavedJob> SavedByUsers { get; set; } = new HashSet<SavedJob>();
    }
}