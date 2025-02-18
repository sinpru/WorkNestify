using System.ComponentModel.DataAnnotations;

namespace WorkNestify.DataAccess.Entities.Companies
{
    public class CompanySize
    {
        [Key] public int Id { get; set; }

        [Required]
        [Display(Name = "Company Size")]
        public string Name { get; set; }

        public ICollection<Company> Companies { get; set; } = new HashSet<Company>();
    }
}