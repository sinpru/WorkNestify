using System.ComponentModel.DataAnnotations;

namespace WorkNestify.DataAccess.Entities.Jobs
{
    public class JobType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job Type")]
        public string Name { get; set; }

        public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }
}