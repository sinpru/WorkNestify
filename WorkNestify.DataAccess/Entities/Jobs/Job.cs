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
        [Display(Name = "Job Type")]
        public int JobTypeId { get; set; }

        [ForeignKey(nameof(JobTypeId))]
        [BindNever]
        [Display(Name = "Job Type")]
        public JobType? JobType { get; set; }

        [Required]
        [Display(Name = "Job Status")]
        public int JobStatusId { get; set; } = 3;

        [ForeignKey(nameof(JobStatusId))]
        [BindNever]
        [Display(Name = "Job Status")]
        public JobStatus? JobStatus { get; set; }

        [Required]
        [Display(Name = "Job Level")]
        public int JobLevelId { get; set; }

        [ForeignKey(nameof(JobLevelId))]
        [BindNever]
        [Display(Name = "Job Level")]
        public JobLevel? JobLevel { get; set; }

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
        public int ProvinceId { get; set; }
        
        [ForeignKey(nameof(ProvinceId))]
        [BindNever]
        [Display(Name = "Province")]
        public Province? Province { get; set; }
        
        [Required]
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        
        [ForeignKey(nameof(DistrictId))]
        [BindNever]
        [Display(Name = "District")]
        public District? District { get; set; }
        
        [Required]
        [Display(Name = "Ward")]
        public string WardCode { get; set; }
        
        [ForeignKey(nameof(WardCode))]
        [BindNever]
        [Display(Name = "Ward")]
        public Ward? Ward { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
    }
}