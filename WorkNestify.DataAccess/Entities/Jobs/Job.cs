using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Entities.JobApplications;

namespace WorkNestify.DataAccess.Entities.Jobs
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
        [Display(Name = "Location")]
        public string Location { get; set; }

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

        [Required]
        public int JobTypeId { get; set; }

        [ForeignKey(nameof(JobTypeId))]
        [Display(Name = "Job Type")]
        public JobType? JobType { get; set; }

        [Required]
        public int JobStatusId { get; set; } = 3;

        [ForeignKey(nameof(JobStatusId))]
        [Display(Name = "Job Status")]
        public JobStatus? JobStatus { get; set; }

        [Required]
        public int JobLevelId { get; set; }

        [ForeignKey(nameof(JobLevelId))]
        [Display(Name = "Job Level")]
        public JobLevel? JobLevel { get; set; }

        [Required]
        public int JobCategoryId { get; set; }

        [ForeignKey(nameof(JobCategoryId))]
        [Display(Name = "Job Category")]
        public JobCategory? JobCategory { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Display(Name = "Company")]
        public Company? Company { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
    }
}