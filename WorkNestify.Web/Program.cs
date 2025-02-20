using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using WorkNestify.DataAccess.Entities.Users;
using WorkNestify.DataAccess.Repositories.Implementations;
using WorkNestify.DataAccess.Repositories.Implementations.Companies;
using WorkNestify.DataAccess.Repositories.Implementations.JobApplications;
using WorkNestify.DataAccess.Repositories.Implementations.Jobs;
using WorkNestify.DataAccess.Repositories.Implementations.Users;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.DataAccess.Repositories.Interfaces.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Users;
using WorkNestify.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register EmailSender
builder.Services.AddTransient<IEmailSender, EmailSender>();

// Setting up Identity 
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager<SignInManager<IdentityUser>>();

// Configure application cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// External Authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddCookie()
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    })
    .AddFacebook(facebookOptions =>
    {
        facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
        facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
    });



// Repository Structure Implementation
// Companies
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyReviewRepository, CompanyReviewRepository>();
builder.Services.AddScoped<ICompanySizeRepository, CompanySizeRepository>();

// JobApplications
builder.Services.AddScoped<IJobApplicationRepository, JobApplicationRepository>();
builder.Services.AddScoped<IJobApplicationStatusRepository, JobApplicationStatusRepository>();

// Jobs
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobCategoryRepository, JobCategoryRepository>();
builder.Services.AddScoped<IJobLevelRepository, JobLevelRepository>();
builder.Services.AddScoped<IJobStatusRepository, JobStatusRepository>();
builder.Services.AddScoped<IJobTypeRepository, JobTypeRepository>();

// Users
builder.Services.AddScoped<IEmployerRepository, EmployerRepository>();
builder.Services.AddScoped<IJobSeekerRepository, JobSeekerRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Middlewares
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Defining routes
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=JobSeeker}/{controller=Home}/{action=Index}/{id?}");

app.Run();