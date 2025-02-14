using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Entities.Users;

namespace WorkNestify.DataAccess.Entities.JobApplications
{
    public class JobApplication
    {
        [Key]
        public int JobApplicationID { get; set; }
        
        [Required]
        [Column(TypeName = "TEXT")]
        [Display(Name = "Cover Letter")]
        public string CoverLetter { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "DATETIME")]
        public DateTime ApplicationDate { get; set; } = DateTime.Now;
        
        [DataType(DataType.DateTime)]
        [Column(TypeName = "DATETIME")]
        public DateTime? ModifiedDate { get; set; } = DateTime.Now;
        
        [Required]
        [Display(Name ="Job Seeker ID")]
        public string JobSeekerID { get; set; }
        
        [ForeignKey(nameof(JobSeekerID))]
        [Display(Name = "Job Seeker")]
        public JobSeeker JobSeeker { get; set; }
        
        [Required]
        [Display(Name = "Job ID")]
        public int JobID { get; set; }
        
        [ForeignKey(nameof(JobID))]
        [Display(Name = "Job")]
        public Job Job { get; set; }
        
        [Required]
        [Display(Name = "Job Application Status ID")]
        public int JobApplicationStatusID { get; set; }
        
        [ForeignKey(nameof(JobApplicationStatusID))]
        [Display(Name = "Job Application Status")]
        public JobApplicationStatus JobApplicationStatus { get; set; }
    }
}