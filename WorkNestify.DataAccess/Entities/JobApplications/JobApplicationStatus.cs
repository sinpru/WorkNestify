using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkNestify.DataAccess.Entities.JobApplications
{
    public class JobApplicationStatus
    {
        [Key]
        public int JobApplicationStatusID { get; set; }
        
        [Required]
        [Column(TypeName = "TEXT")]
        [Display(Name = "Status Name")]
        public string StatusName { get; set; }
        
        public List<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}