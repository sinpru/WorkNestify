using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WorkNestify.DataAccess.Data;
using WorkNestify.DataAccess.DbInitializer.Seeds;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.Models.Models.Users;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.DataAccess.DbInitializer;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public DbInitializer(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IUnitOfWork unitOfWork,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task Initialize()
    {
        // Apply migrations if pending
        try
        {
            if (_unitOfWork.Context.Database.GetPendingMigrations().Any())
            {
                await _unitOfWork.Context.Database.MigrateAsync();
            }
        }
        catch (Exception)
        {
            throw;
        }

        // Create roles if they do not exist
        if (!await _roleManager.RoleExistsAsync(Roles.JobSeeker))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.JobSeeker));
            await _roleManager.CreateAsync(new IdentityRole(Roles.Employer));
            await _roleManager.CreateAsync(new IdentityRole(Roles.Admin));

            // Create admin user
            var adminUser = new ApplicationUser
            {
                UserName = _configuration["AdminAccount:AccountEmail"],
                Email = _configuration["AdminAccount:AccountEmail"],
                FullName = _configuration["AdminAccount:AccountName"],
                PhoneNumber = _configuration["AdminAccount:AccountPhoneNumber"]
            };

            var adminAdded = await _userManager.CreateAsync(adminUser, _configuration["AdminAccount:AccountPassword"]);
            if (adminAdded.Succeeded)
            {
                await _unitOfWork.SaveAsync(); // Save user changes
                var user = _unitOfWork.Context.ApplicationUsers.FirstOrDefault(u => u.Email == _configuration["AdminAccount:AccountEmail"]);
                if (user != null)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin);
                }
                else
                {
                    throw new Exception("Admin user was created but could not be found in the database.");
                }
            }
            else
            {
                throw new Exception("Failed to create admin user: " + string.Join(", ", adminAdded.Errors.Select(e => e.Description)));
            }
        }

        // Seed data for entities
        await SeedEntities();
    }

    public async Task SeedEntities()
    {
        // Seed Job Categories
        if (await _unitOfWork.JobCategories.CountAsync(null) == 0)
        {
            await _unitOfWork.JobCategories.AddRangeAsync(JobCategorySeed.GetJobCategories());
            await _unitOfWork.SaveAsync();
        }

        // Seed Companies
        if (await _unitOfWork.Companies.CountAsync(null) == 0)
        {
            await _unitOfWork.Companies.AddRangeAsync(CompanySeed.GetCompanies());
            await _unitOfWork.SaveAsync();
        }

        // Seed Jobs
        if (await _unitOfWork.Jobs.CountAsync(null) == 0)
        {
            await _unitOfWork.Jobs.AddRangeAsync(JobSeed.GetJobs());
            await _unitOfWork.SaveAsync();
        }
    }
}