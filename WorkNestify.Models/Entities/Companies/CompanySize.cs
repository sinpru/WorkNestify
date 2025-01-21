using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkNestify.Models.Entities.Companies;

public class CompanySize
{
    [Key]
    public int CompanySizeID { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Company Size Name")]
    public string CompanySizeName { get; set; }
}