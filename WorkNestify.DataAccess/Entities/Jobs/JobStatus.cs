using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkNestify.DataAccess.Entities.Jobs;

public class JobStatus
{
    [Key]
    public int JobStatusID { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Job Status Name")]
    public string JobStatusName { get; set; }

    public List<Job> Jobs { get; set; } = new List<Job>();
}