using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkNestify.DataAccess.Entities.Companies;

namespace WorkNestify.DataAccess.Entities.Users;

public class Employer : ApplicationUser
{
    [Required]
    [Display(Name = "Company ID")]
    public int CompanyID { get; set; }

    [ForeignKey(nameof(CompanyID))]
    [Display(Name = "Company")]
    public Company Company { get; set; } = null!;
}