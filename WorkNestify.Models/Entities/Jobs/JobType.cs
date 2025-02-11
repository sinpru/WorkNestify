using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkNestify.Models.Entities.Jobs;

public class JobType
{
    [Key]
    public int JobTypeID { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Job Type Name")]
    public string JobTypeName { get; set; }
    
    public List<Job> Jobs { get; set; } = new List<Job>();
}