using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WorkNestify.DataAccess.Entities.Users;

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
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        public string ReviewerId { get; set; }
        
        [ForeignKey(nameof(ReviewerId))]
        [Display(Name = "Reviewer")]
        [BindNever]
        public ApplicationUser? Reviewer { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Display(Name = "Company")]
        [BindNever]
        public Company? Company { get; set; }
    }
}