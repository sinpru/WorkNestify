using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkNestify.Models.Entities.Companies;

public class Company
{
    [Key]
    public int CompanyID { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Company Name")]
    public string CompanyName { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Website URL")]
    public string Website { get; set; }

    [Required]
    [EmailAddress]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [Required]
    [Phone]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Address")]
    public string Address { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Company Description")]
    public string CompanyDescription { get; set; }

    [Column(TypeName = "TEXT")]
    [Display(Name = "Logo URL")]
    public string Logo { get; set; }

    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Industry")]
    public string Industry { get; set; }

    [Column(TypeName = "DATE")]
    [Display(Name = "Founded Date")]
    public DateTime? FoundedDate { get; set; }

    [Required]
    [Column(TypeName = "DATETIME")]
    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    [Required]
    [Column(TypeName = "DATETIME")]
    [Display(Name = "Modified Date")]
    public DateTime ModifiedDate { get; set; } = DateTime.Now;

    [Required]
    [Display(Name = "Company Size ID")]
    public int CompanySizeID { get; set; }

    [ForeignKey(nameof(CompanySizeID))]
    [Display(Name = "Company Size")]
    public CompanySize CompanySize { get; set; }
}