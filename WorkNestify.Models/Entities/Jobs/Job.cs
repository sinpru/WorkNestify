using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkNestify.Models.Entities.Companies;
using WorkNestify.Models.Entities.JobApplications;

namespace WorkNestify.Models.Entities.Jobs;

public class Job
{
    [Key]
    public int JobID { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Job Title")]
    public string JobTitle { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Job Description")]
    public string JobDescription { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Location")]
    public string Location { get; set; }

    [Required]
    [Display(Name = "Salary")]
    public double Salary { get; set; }

    [Column(TypeName = "DATETIME")]
    [Display(Name = "Start Date")]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "DATETIME")]
    [Display(Name = "End Date")]
    public DateTime? EndDate { get; set; }

    [Required]
    [Column(TypeName = "DATETIME")]
    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Required]
    [Column(TypeName = "DATETIME")]
    [Display(Name = "Modified Date")]
    public DateTime ModifiedDate { get; set; } = DateTime.Now;

    [Required]
    [Display(Name = "Job Type ID")]
    public int JobTypeID { get; set; }

    [ForeignKey(nameof(JobTypeID))]
    [Display(Name = "Job Type")]
    public JobType JobType { get; set; }

    [Required]
    [Display(Name = "Job Status ID")]
    public int JobStatusID { get; set; }

    [ForeignKey(nameof(JobStatusID))]
    [Display(Name = "Job Status")]
    public JobStatus JobStatus { get; set; }
    
    [Required]
    [Display(Name = "Company ID")]
    public int CompanyID { get; set; }
    
    [ForeignKey(nameof(CompanyID))]
    [Display(Name = "Company")]
    public Company Company { get; set; }
    
    public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}