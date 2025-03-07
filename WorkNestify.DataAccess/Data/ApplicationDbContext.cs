using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Entities.Companies;
using WorkNestify.DataAccess.Entities.JobApplications;
using WorkNestify.DataAccess.Entities.Jobs;
using WorkNestify.DataAccess.Entities.Locations;
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
    
    // Locations
    public DbSet<Ward> Wards { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Province> Provinces { get; set; }
    
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
        // Job Relationships
        modelBuilder.Entity<Job>()
            .HasOne(j => j.Company)
            .WithMany(c => c.Jobs)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        // JobApplication Relationships
        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.JobSeeker)
            .WithMany(js => js.JobApplications)
            .HasForeignKey(ja => ja.JobSeekerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.Job)
            .WithMany(j => j.JobApplications)
            .HasForeignKey(ja => ja.JobId)
            .OnDelete(DeleteBehavior.Restrict);

        // CompanyReview Relationships
        modelBuilder.Entity<CompanyReview>()
            .HasOne(cr => cr.Company)
            .WithMany(c => c.CompanyReviews)
            .HasForeignKey(cr => cr.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Locations Relationships
        modelBuilder.Entity<Ward>()
            .HasOne(w => w.District)
            .WithMany(d => d.Wards)
            .HasForeignKey(w => w.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Configure DeleteBehavior only, no FK redefinition
        modelBuilder.Entity<Company>()
            .HasOne(c => c.Province)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Company>()
            .HasOne(c => c.District)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Company>()
            .HasOne(c => c.Ward)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<District>()
            .HasOne(d => d.Province)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Province)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.District)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Ward)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict);
    }
}