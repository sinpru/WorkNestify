using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Entities.Jobs;

namespace WorkNestify.DataAccess.Entities.Locations;

public class Ward
{
    [Key]
    public String Code { get; set; }
    
    [Required]
    [Display(Name = "Ward")]
    public string Name { get; set; }
    
    [Required]
    public int DistrictId { get; set; }
    
    [ForeignKey(nameof(DistrictId))]
    [Display(Name = "District")]
    [BindNever]
    public District? District { get; set; }
    
    public ICollection<Company> Companies { get; set; } = new HashSet<Company>();
    public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
}