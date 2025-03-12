using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WorkNestify.Models.Models.Users;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Models.Models.Locations;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.Models.Models.Companies
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Company Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Website URL")]
        public string Website { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }

        [Required]
        [Display(Name = "Company Description")]
        public string Description { get; set; }

        [Display(Name = "Logo URL")]
        public string? Logo { get; set; }

        [Required]
        [Display(Name = "Industry")]
        public string Industry { get; set; }

        [Display(Name = "Founded Date")]
        public DateTime? FoundedDate { get; set; }

        [Required]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Company Size")]
        public string Size { get; set; } = CompanySizes.Small;
        
        [Required]
        [Display(Name = "Province")]
        public int ProvinceId { get; set; }
        
        [Display(Name = "Province")]
        [ForeignKey(nameof(ProvinceId))]
        [BindNever]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Province? Province { get; set; }
        
        [Required]
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        
        [Display(Name = "District")]
        [ForeignKey(nameof(DistrictId))]
        [BindNever]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public District? District { get; set; }
        
        [Required]
        [Display(Name = "Ward")]
        public string WardCode { get; set; }
        
        [Display(Name = "Ward")]
        [ForeignKey(nameof(WardCode))]
        [BindNever]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public Ward? Ward { get; set; }
        
        public ICollection<CompanyReview> CompanyReviews { get; set; } = new HashSet<CompanyReview>();
        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new HashSet<ApplicationUser>();
        public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }
}
