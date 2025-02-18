using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Entities.Users;

namespace WorkNestify.DataAccess.Entities.JobApplications
{
    public class JobApplication
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Cover Letter")]
        public string CoverLetter { get; set; }

        [Required]
        public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string JobSeekerId { get; set; }

        [ForeignKey(nameof(JobSeekerId))]
        public JobSeeker JobSeeker { get; set; }

        [Required]
        public int JobId { get; set; }

        [ForeignKey(nameof(JobId))]
        public Job Job { get; set; }

        [Required]
        public int JobApplicationStatusId { get; set; }

        [ForeignKey(nameof(JobApplicationStatusId))]
        public JobApplicationStatus JobApplicationStatus { get; set; }
    }
}