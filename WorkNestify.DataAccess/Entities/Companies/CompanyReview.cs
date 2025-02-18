using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkNestify.DataAccess.Entities.Companies
{
    public class CompanyReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company Review")]
        public string Content { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars")]
        public float Rating { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        public Company Company { get; set; } = null!;
    }
}