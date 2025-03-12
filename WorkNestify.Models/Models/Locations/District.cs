using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WorkNestify.Models.Models.Companies;
using WorkNestify.Models.Models.Jobs;

namespace WorkNestify.Models.Models.Locations;

public class District
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [Display(Name = "District")]
    public string Name { get; set; }
    
    [Required]
    public int ProvinceId { get; set; }
    
    [ForeignKey(nameof(ProvinceId))]
    [Display(Name = "Province")]
    [BindNever]
    public Province? Province { get; set; }
    
    public ICollection<Ward> Wards { get; set; } = new HashSet<Ward>();
    public ICollection<Company> Companies { get; set; } = new HashSet<Company>();
    public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
}