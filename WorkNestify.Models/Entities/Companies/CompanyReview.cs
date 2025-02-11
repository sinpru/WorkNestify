using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkNestify.Models.Entities.Companies;

public class CompanyReview
{
    [Key]
    public int CompanyReviewID { get; set; }
    
    [Required]
    [Column(TypeName = "TEXT")]
    [Display(Name = "Company Review")]
    public string CompanyReviewContent { get; set; }
    
    [Required]
    [Column(TypeName = "FLOAT")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars")]
    public float Rating { get; set; }
    
    [Required]
    [Display(Name = "Company ID")]
    public int CompanyID { get; set; }
    
    [ForeignKey(nameof(CompanyID))]
    [Display(Name = "Company")]
    public Company Company { get; set; } = null!;
}