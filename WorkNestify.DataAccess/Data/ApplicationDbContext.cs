using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkNestify.Models.Entities.Companies;
using WorkNestify.Models.Entities.Jobs;

namespace WorkNestify.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<CompanySize> CompanySizes { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<JobType> JobTypes { get; set; }
    public DbSet<JobStatus> JobStatuses { get; set; }
    public DbSet<Job> Jobs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CompanySize>().HasData(
            new CompanySize { CompanySizeID = 1, CompanySizeName = "Small" },
            new CompanySize { CompanySizeID = 2, CompanySizeName = "Medium" },
            new CompanySize { CompanySizeID = 3, CompanySizeName = "Large" });
        
        modelBuilder.Entity<JobType>().HasData(
            new JobType { JobTypeID = 1, JobTypeName = "Full-Time" },
            new JobType { JobTypeID = 2, JobTypeName = "Part-Time" },
            new JobType { JobTypeID = 3, JobTypeName = "Freelance" },
            new JobType { JobTypeID = 4, JobTypeName = "Remote" });

        modelBuilder.Entity<JobStatus>().HasData(
            new JobStatus { JobStatusID = 1, JobStatusName = "Open" },
            new JobStatus { JobStatusID = 2, JobStatusName = "Closed" },
            new JobStatus { JobStatusID = 3, JobStatusName = "Pending" },
            new JobStatus { JobStatusID = 4, JobStatusName = "Expired" });
    }
}