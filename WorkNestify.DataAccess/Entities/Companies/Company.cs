using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Entities.Locations;
using WorkNestify.DataAccess.Entities.Users;

namespace WorkNestify.DataAccess.Entities.Companies
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
        
        [Required]
        [Display(Name = "Company Size")]
        public int CompanySizeId { get; set; }

        [ForeignKey(nameof(CompanySizeId))]
        [BindNever]
        public CompanySize? CompanySize { get; set; }
        
        [Required]
        [Display(Name = "Province")]
        public int ProvinceId { get; set; }
        
        [ForeignKey(nameof(ProvinceId))]
        [BindNever]
        public Province? Province { get; set; }
        
        [Required]
        [Display(Name = "District")]
        public int DistrictId { get; set; }
        
        [ForeignKey(nameof(DistrictId))]
        [BindNever]
        public District? District { get; set; }
        
        [Required]
        [Display(Name = "Ward")]
        public string WardCode { get; set; }
        
        [ForeignKey(nameof(WardCode))]
        [BindNever]
        public Ward? Ward { get; set; }
        
        public ICollection<CompanyReview> CompanyReviews { get; set; } = new HashSet<CompanyReview>();
        public ICollection<Employer> Employers { get; set; } = new HashSet<Employer>();
        public ICollection<Job> Jobs { get; set; } = new HashSet<Job>();
    }
}
