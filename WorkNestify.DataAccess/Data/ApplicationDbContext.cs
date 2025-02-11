using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkNestify.Models.Entities.Companies;
using WorkNestify.Models.Entities.Jobs;
using WorkNestify.Models.Entities.JobApplications;
using WorkNestify.Models.Entities.Users;

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
    public DbSet<JobStatus> JobStatuses { get; set; }
    
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
            new CompanySize { CompanySizeID = 1, CompanySizeName = "Small" },
            new CompanySize { CompanySizeID = 2, CompanySizeName = "Medium" },
            new CompanySize { CompanySizeID = 3, CompanySizeName = "Large" });
        
        // Seed Data for JobType
        modelBuilder.Entity<JobType>().HasData(
            new JobType { JobTypeID = 1, JobTypeName = "Full-Time" },
            new JobType { JobTypeID = 2, JobTypeName = "Part-Time" },
            new JobType { JobTypeID = 3, JobTypeName = "Freelance" },
            new JobType { JobTypeID = 4, JobTypeName = "Remote" });

        // Seed Data for JobStatus
        modelBuilder.Entity<JobStatus>().HasData(
            new JobStatus { JobStatusID = 1, JobStatusName = "Open" },
            new JobStatus { JobStatusID = 2, JobStatusName = "Closed" },
            new JobStatus { JobStatusID = 3, JobStatusName = "Pending" },
            new JobStatus { JobStatusID = 4, JobStatusName = "Expired" });
        
        // Relationships
        modelBuilder.Entity<Job>()
            .HasOne(j => j.JobType)
            .WithMany()
            .HasForeignKey(j => j.JobTypeID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.JobStatus)
            .WithMany()
            .HasForeignKey(j => j.JobStatusID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Job>()
            .HasOne(j => j.Company)
            .WithMany()
            .HasForeignKey(j => j.CompanyID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.JobSeeker)
            .WithMany(js => js.JobApplications)
            .HasForeignKey(ja => ja.JobSeekerID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.Job)
            .WithMany()
            .HasForeignKey(ja => ja.JobID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.JobApplicationStatus)
            .WithMany()
            .HasForeignKey(ja => ja.JobApplicationStatusID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CompanyReview>()
            .HasOne(cr => cr.Company)
            .WithMany()
            .HasForeignKey(cr => cr.CompanyID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}