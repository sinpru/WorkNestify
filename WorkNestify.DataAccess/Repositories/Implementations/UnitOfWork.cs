using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.DataAccess.Repositories.Interfaces.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Locations;
using WorkNestify.DataAccess.Repositories.Interfaces.Users;
// using WorkNestify.DataAccess.Repositories.Interfaces.Users;
using WorkNestify.Models.Models.Locations;

namespace WorkNestify.DataAccess.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public ApplicationDbContext Context => _context;

    // Companies
    public ICompanyRepository Companies { get; private set; }
    public ICompanyReviewRepository CompanyReviews { get; private set; }

    // Jobs
    public IJobRepository Jobs { get; private set; }
    public IJobCategoryRepository JobCategories { get; private set; }

    // JobApplications
    public IJobApplicationRepository JobApplications { get; private set; }
    
    // Locations
    public IDistrictRepository Districts { get; private set; }
    public IProvinceRepository Provinces { get; private set; }
    public IWardRepository Wards { get; private set; }

    // Users
    public IApplicationUserRepository ApplicationUser { get; }

    public UnitOfWork(ApplicationDbContext context,
        ICompanyRepository companyRepository,
        ICompanyReviewRepository companyReviewRepository,
        IJobRepository jobRepository,
        IJobCategoryRepository jobCategoryRepository,
        IJobApplicationRepository jobApplicationRepository,
        IDistrictRepository districtRepository,
        IProvinceRepository provinceRepository,
        IWardRepository wardRepository,
        IApplicationUserRepository applicationUser)
    {
        _context = context;
        Companies = companyRepository;
        CompanyReviews = companyReviewRepository;
        Jobs = jobRepository;
        JobCategories = jobCategoryRepository;
        JobApplications = jobApplicationRepository;
        Districts = districtRepository;
        Provinces = provinceRepository;
        Wards = wardRepository;
        ApplicationUser = applicationUser;
    }

    public async Task SaveAsync()
    {
        try
        {
            // Save Provinces
            var provincesToAdd = _context.ChangeTracker.Entries<Province>()
                .Where(e => e.State == EntityState.Added)
                .ToList();
            if (provincesToAdd.Any())
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Provinces ON");
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Provinces OFF");
            }

            // Save Districts
            var districtsToAdd = _context.ChangeTracker.Entries<District>()
                .Where(e => e.State == EntityState.Added)
                .ToList();
            if (districtsToAdd.Any())
            {
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Districts ON");
                await _context.SaveChangesAsync();
                await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Districts OFF");
            }

            // Save Wards
            var wardsToAdd = _context.ChangeTracker.Entries<Ward>()
                .Where(e => e.State == EntityState.Added)
                .ToList();
            if (wardsToAdd.Any())
            {
                await _context.SaveChangesAsync();
            }

            // Save remaining changes
            if (_context.ChangeTracker.HasChanges())
            {
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SaveAsync Error: {ex.Message}");
            // Log entity states for debugging
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                Console.WriteLine($"Entity: {entry.Entity}, State: {entry.State}");
            }
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Provinces OFF");
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Districts OFF");
            throw;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}