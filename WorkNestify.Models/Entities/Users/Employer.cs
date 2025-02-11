using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkNestify.Models.Entities.Companies;

namespace WorkNestify.Models.Entities.Users;

[Table("Employers")]
public class Employer : ApplicationUser
{
    [Required]
    [Display(Name = "Company ID")]
    public int CompanyID { get; set; }

    [ForeignKey(nameof(CompanyID))]
    [Display(Name = "Company")]
    public Company Company { get; set; } = null!;
}