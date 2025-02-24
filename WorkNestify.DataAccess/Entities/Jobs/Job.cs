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
        public string Location { get; set; }

        [Required]
        public double Salary { get; set; }

        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }
        
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Job Type")]
        public int JobTypeId { get; set; }

        [ForeignKey(nameof(JobTypeId))]
        public JobType JobType { get; set; }

        [Required]
        [Display(Name = "Job Status")]
        public int JobStatusId { get; set; }

        [ForeignKey(nameof(JobStatusId))]
        public JobStatus JobStatus { get; set; }

        [Required]
        [Display(Name = "Job Level")]
        public int JobLevelId { get; set; }

        [ForeignKey(nameof(JobLevelId))]
        public JobLevel JobLevel { get; set; }

        [Required]
        [Display(Name = "Job Category")]
        public int JobCategoryId { get; set; }

        [ForeignKey(nameof(JobCategoryId))]
        public JobCategory JobCategory { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
    }
}