using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkNestify.Models.Entities.Jobs;

public class JobStatus
{
    [Key]
    public int JobStatusID { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Job Status Name")]
    public string JobStatusName { get; set; }
}