using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WorkNestify.DataAccess.Data;
using WorkNestify.Models.Models.Users;
using WorkNestify.Utilities.Constants;

namespace WorkNestify.DataAccess.DbInitializer;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public DbInitializer(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext context,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _configuration = configuration;
    }
    
    public void Initialize()
    {
        // Apply migrations if pending
        try
        {
            if (_context.Database.GetPendingMigrations().Count() > 0)
            {
                _context.Database.Migrate();
            }
        }
        catch (Exception)
        {
            throw;
        }
        
        // Create roles if they do not exist
        if (!_roleManager.RoleExistsAsync(Roles.JobSeeker).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(Roles.JobSeeker)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Employer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Staff)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();
            
            // Create admin user
            var adminUser = new ApplicationUser
            {
                UserName = _configuration["AdminAccount:AccountEmail"],
                Email = _configuration["AdminAccount:AccountEmail"],
                FullName = _configuration["AdminAccount:AccountName"],
                PhoneNumber = _configuration["AdminAccount:AccountPhoneNumber"]
            };

            var adminAdded = _userManager.CreateAsync(adminUser, _configuration["AdminAccount:AccountPassword"]).GetAwaiter().GetResult();
            if (adminAdded.Succeeded)
            {
                _context.SaveChanges(); // Ensure the user is committed to the database
                var user = _context.ApplicationUsers.FirstOrDefault(u => u.Email == _configuration["AdminAccount:AccountEmail"]);
                if (user != null)
                {
                    _userManager.AddToRoleAsync(user, Roles.JobSeeker).GetAwaiter().GetResult();
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
    }
}