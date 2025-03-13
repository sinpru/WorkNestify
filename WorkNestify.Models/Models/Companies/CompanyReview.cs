using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WorkNestify.Models.Models.Users;

namespace WorkNestify.Models.Models.Companies
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
        public string ApplicationUserId { get; set; }
        
        [ForeignKey(nameof(ApplicationUserId))]
        [Display(Name = "Reviewer")]
        [BindNever]
        public ApplicationUser? ApplicationUser { get; set; }

        [Required]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [ForeignKey(nameof(CompanyId))]
        [Display(Name = "Company")]
        [BindNever]
        public Company? Company { get; set; }
    }
}