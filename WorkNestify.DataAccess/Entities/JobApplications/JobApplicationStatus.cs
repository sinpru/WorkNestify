using System.ComponentModel.DataAnnotations;

namespace WorkNestify.DataAccess.Entities.JobApplications
{
    public class JobApplicationStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Status Name")]
        public string Name { get; set; }

        public ICollection<JobApplication> JobApplications { get; set; } = new HashSet<JobApplication>();
    }
}