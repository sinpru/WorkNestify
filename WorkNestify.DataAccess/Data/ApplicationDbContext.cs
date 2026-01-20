using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkNestify.Models.Models.Companies;
using WorkNestify.Models.Models.JobApplications;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Models.Models.Users;

namespace WorkNestify.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Companies
    public DbSet<Company> Companies { get; set; }
    public DbSet<CompanyReview> CompanyReviews { get; set; }

    // Jobs
    public DbSet<Job> Jobs { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }

    // Job Applications
    public DbSet<JobApplication> JobApplications { get; set; }

    // Users
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<SavedJob> SavedJobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relationships
        // Job Relationships
        modelBuilder.Entity<Job>()
            .HasOne(j => j.Company)
            .WithMany(c => c.Jobs)
            .HasForeignKey(j => j.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        // JobApplication Relationships
        modelBuilder.Entity<JobApplication>()
            .HasOne(ja => ja.ApplicationUser)
            .WithMany(js => js.JobApplications)
            .HasForeignKey(ja => ja.ApplicationUserId)
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

        // SavedJob Relationships
        modelBuilder.Entity<SavedJob>()
            .HasKey(sj => new { sj.UserId, sj.JobId });

        modelBuilder.Entity<SavedJob>()
            .HasOne(sj => sj.User)
            .WithMany(u => u.SavedJobs)
            .HasForeignKey(sj => sj.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SavedJob>()
            .HasOne(sj => sj.Job)
            .WithMany(j => j.SavedByUsers)
            .HasForeignKey(sj => sj.JobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}