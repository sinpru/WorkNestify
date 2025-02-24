using System.ComponentModel.DataAnnotations;

namespace WorkNestify.DataAccess.Entities.Jobs
{
    public class JobLevel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job Level")]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }
}