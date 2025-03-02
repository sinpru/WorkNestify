using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Entities.JobApplications;
using WorkNestify.DataAccess.Entities.Locations;

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

        [Required]
        public int JobTypeId { get; set; }

        [ForeignKey(nameof(JobTypeId))]
        [Display(Name = "Job Type")]
        [BindNever]
        public JobType? JobType { get; set; }

        [Required]
        public int JobStatusId { get; set; } = 3;

        [ForeignKey(nameof(JobStatusId))]
        [Display(Name = "Job Status")]
        [BindNever]
        public JobStatus? JobStatus { get; set; }

        [Required]
        public int JobLevelId { get; set; }

        [ForeignKey(nameof(JobLevelId))]
        [Display(Name = "Job Level")]
        [BindNever]
        public JobLevel? JobLevel { get; set; }

        [Required]
        public int JobCategoryId { get; set; }

        [ForeignKey(nameof(JobCategoryId))]
        [Display(Name = "Job Category")]
        [BindNever]
        public JobCategory? JobCategory { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Display(Name = "Company")]
        [BindNever]
        public Company? Company { get; set; }
        
        [Required]
        public int ProvinceId { get; set; }
        
        [ForeignKey(nameof(ProvinceId))]
        [Display(Name = "Province")]
        [BindNever]
        public Province? Province { get; set; }
        
        [Required]
        public int DistrictId { get; set; }
        
        [ForeignKey(nameof(DistrictId))]
        [Display(Name = "District")]
        [BindNever]
        public District? District { get; set; }
        
        [Required]
        public int WardId { get; set; }
        
        [ForeignKey(nameof(WardId))]
        [Display(Name = "Ward")]
        [BindNever]
        public Ward? Ward { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
    }
}