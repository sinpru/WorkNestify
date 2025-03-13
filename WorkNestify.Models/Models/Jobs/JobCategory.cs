using System.ComponentModel.DataAnnotations;

namespace WorkNestify.Models.Models.Jobs
{
    public class JobCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Job Category")]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }
}