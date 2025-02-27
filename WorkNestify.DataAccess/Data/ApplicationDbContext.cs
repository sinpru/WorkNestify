using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Entities.JobApplications;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Entities.Users;

namespace WorkNestify.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Companies
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanySize> CompanySizes { get; set; }
    public DbSet<CompanyReview> CompanyReviews { get; set; }
    
    // Jobs
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobType> JobTypes { get; set; }
    public DbSet<JobLevel> JobLevels { get; set; }
    public DbSet<JobStatus> JobStatuses { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }
    
    // Job Applications
    public DbSet<JobApplication> JobApplications { get; set; }
    public DbSet<JobApplicationStatus> JobApplicationStatuses { get; set; }
    
    // Users
    public DbSet<JobSeeker> JobSeekers { get; set; }
    public DbSet<Employer> Employers { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Data for CompanySize
        modelBuilder.Entity<CompanySize>().HasData(
            new CompanySize { Id = 1, Name = "Small" },
            new CompanySize { Id = 2, Name = "Medium" },
            new CompanySize { Id = 3, Name = "Large" });
        
        // Seed Data for Company
        modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    Id = 1,
                    Name = "FPT Software",
                    Website = "https://www.fpt-software.com",
                    Email = "contact@fpt-software.com",
                    Phone = "+84 24 7300 7300",
                    Address = "Hanoi, Vietnam",
                    Description = "<p>FPT Software is a global technology and IT services provider headquartered in Vietnam.</p>",
                    Logo = "https://upload.wikimedia.org/wikipedia/commons/1/11/FPT_logo_2010.svg",
                    Industry = "Information Technology",
                    FoundedDate = new DateTime(1999, 9, 13),
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    CompanySizeId = 3 // Large
                },
                new Company
                {
                    Id = 2,
                    Name = "VNG Corporation",
                    Website = "https://www.vng.com.vn",
                    Email = "support@vng.com.vn",
                    Phone = "+84 28 3962 3888",
                    Address = "Ho Chi Minh City, Vietnam",
                    Description = "<p>VNG is a leading technology company in Vietnam, known for its digital entertainment, cloud services, and fintech solutions.</p>",
                    Logo = "https://upload.wikimedia.org/wikipedia/commons/8/8f/VNG_Corp._logo.svg",
                    Industry = "Technology & Entertainment",
                    FoundedDate = new DateTime(2004, 9, 9),
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    CompanySizeId = 3
                },
                new Company
                {
                    Id = 3,
                    Name = "Tiki.vn",
                    Website = "https://www.tiki.vn",
                    Email = "contact@tiki.vn",
                    Phone = "+84 1900 6035",
                    Address = "Ho Chi Minh City, Vietnam",
                    Description = "<p>Tiki is one of the biggest e-commerce platforms in Vietnam, offering a wide range of products and fast delivery services.</p>",
                    Logo = "https://upload.wikimedia.org/wikipedia/commons/4/43/Logo_Tiki_2023.png",
                    Industry = "E-commerce",
                    FoundedDate = new DateTime(2010, 3, 3),
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    CompanySizeId = 2
                },
                new Company
                {
                    Id = 4,
                    Name = "VinAI Research",
                    Website = "https://www.vinai.io",
                    Email = "info@vinai.io",
                    Phone = "+84 24 7108 7788",
                    Address = "Hanoi, Vietnam",
                    Description = "<p>VinAI is an AI research lab established by Vingroup, focusing on artificial intelligence applications.</p>",
                    Logo = "https://www.vinai.io/wp-content/uploads/2021/12/logo-1.png",
                    Industry = "Artificial Intelligence",
                    FoundedDate = new DateTime(2019, 6, 10),
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                    CompanySizeId = 1
                }
            );
        
        // Seed Data for JobType
        modelBuilder.Entity<JobType>().HasData(
            new JobType { Id = 1, Name = "Full-Time" },
            new JobType { Id = 2, Name = "Part-Time" },
            new JobType { Id = 3, Name = "Freelance" },
            new JobType { Id = 4, Name = "Remote" });

        // Seed Data for JobStatus
        modelBuilder.Entity<JobStatus>().HasData(
            new JobStatus { Id = 1, Name = "Open" },
            new JobStatus { Id = 2, Name = "Closed" },
            new JobStatus { Id = 3, Name = "Pending" },
            new JobStatus { Id = 4, Name = "Expired" });
        
        // Seed Data for JobLevel
        modelBuilder.Entity<JobLevel>().HasData(
            new JobLevel { Id = 1, Name = "Intern", Description = "An entry-level position for individuals who are gaining work experience in their field, typically through an internship program." },
            new JobLevel { Id = 2, Name = "Fresher", Description = "A recent graduate or someone who is new to the job market, with limited professional experience." },
            new JobLevel { Id = 3, Name = "Junior", Description = "A role for individuals with some experience in their field, typically 1-3 years, and who require supervision and guidance." },
            new JobLevel { Id = 4, Name = "Senior", Description = "An experienced professional with significant expertise in the field, typically with over 5 years of experience, often responsible for leading teams or projects." });
        
        // Seed Data for Job Category
        modelBuilder.Entity<JobCategory>().HasData(
            new JobCategory { Id = 1, Name = "Information Technology", Description = "Software development, cybersecurity, networking, and IT support" },
            new JobCategory { Id = 2, Name = "Marketing", Description = "Digital marketing, SEO, content creation, and branding" },
            new JobCategory { Id = 3, Name = "Sales", Description = "Business development, B2B/B2C sales, and customer relationship management" },
            new JobCategory { Id = 4, Name = "Healthcare", Description = "Medical professionals, nursing, pharmaceuticals, and hospital administration" },
            new JobCategory { Id = 5, Name = "Finance & Accounting", Description = "Accounting, auditing, financial analysis, and banking" },
            new JobCategory { Id = 6, Name = "Human Resources", Description = "Recruitment, employee relations, and HR management" },
            new JobCategory { Id = 7, Name = "Engineering", Description = "Mechanical, electrical, civil, and software engineering" },
            new JobCategory { Id = 8, Name = "Education & Training", Description = "Teaching, tutoring, and corporate training" },
            new JobCategory { Id = 9, Name = "Customer Service", Description = "Call center, technical support, and client relations" },
            new JobCategory { Id = 10, Name = "Logistics & Supply Chain", Description = "Transportation, inventory management, and procurement" });
        
        // Relationships
        modelBuilder.Entity<Job>()
            .HasOne(j => j.JobType)
            .WithMany()
            .HasForeignKey(j => j.JobTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.JobStatus)
            .WithMany()
            .HasForeignKey(j => j.JobStatusId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Job>()
            .HasOne(j => j.JobLevel)
            .WithMany()
            .HasForeignKey(j => j.JobLevelId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Job>()
            .HasOne(j => j.JobCategory)
            .WithMany()
            .HasForeignKey(j => j.JobCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Company)
            .WithMany()
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.JobSeeker)
            .WithMany(js => js.JobApplications)
            .HasForeignKey(ja => ja.JobSeekerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.Job)
            .WithMany()
            .HasForeignKey(ja => ja.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.JobApplicationStatus)
            .WithMany()
            .HasForeignKey(ja => ja.JobApplicationStatusId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CompanyReview>()
            .HasOne(cr => cr.Company)
            .WithMany()
            .HasForeignKey(cr => cr.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}