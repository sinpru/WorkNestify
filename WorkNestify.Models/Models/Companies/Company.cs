using System.ComponentModel.DataAnnotations;
using WorkNestify.Models.Models.Users;
using WorkNestify.Models.Models.Jobs;
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
        public int ProvinceCode { get; set; }
        
        [Required]
        [Display(Name = "Ward")]
        public int WardCode { get; set; }
        
        public ICollection<CompanyReview> CompanyReviews { get; set; } = new HashSet<CompanyReview>();
        public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new HashSet<ApplicationUser>();
        public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }
}
