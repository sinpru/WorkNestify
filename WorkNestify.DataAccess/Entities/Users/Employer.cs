using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WorkNestify.DataAccess.Entities.Companies;

namespace WorkNestify.DataAccess.Entities.Users;

public class Employer : ApplicationUser
{
    [Required]
    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    [Display(Name = "Company")]
    public Company? Company { get; set; } = null!;
}