using System.ComponentModel.DataAnnotations;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Entities.Jobs;

namespace WorkNestify.DataAccess.Entities.Locations;

public class Province
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "Province")]
    public string Name { get; set; }
    
    public ICollection<District> Districts { get; set; } = new HashSet<District>();
    public ICollection<Company> Companies { get; set; } = new HashSet<Company>();
    public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
}