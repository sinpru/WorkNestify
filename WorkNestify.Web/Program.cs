using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WorkNestify.DataAccess.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WorkNestify.DataAccess.DbInitializer;
using WorkNestify.DataAccess.Repositories.Implementations;
using WorkNestify.DataAccess.Repositories.Implementations.Companies;
using WorkNestify.DataAccess.Repositories.Implementations.JobApplications;
using WorkNestify.DataAccess.Repositories.Implementations.Jobs;
using WorkNestify.DataAccess.Repositories.Implementations.Locations;
using WorkNestify.DataAccess.Repositories.Interfaces;
using WorkNestify.DataAccess.Repositories.Interfaces.Companies;
using WorkNestify.DataAccess.Repositories.Interfaces.JobApplications;
using WorkNestify.DataAccess.Repositories.Interfaces.Jobs;
using WorkNestify.DataAccess.Repositories.Interfaces.Locations;
using WorkNestify.Models.Models.Users;
using WorkNestify.Services;
using WorkNestify.Utilities;
using WorkNestify.Web.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add SignalR
builder.Services.AddSignalR();

// Add Razor Pages
builder.Services.AddRazorPages();

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.MaxDepth = 128;
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Internal Services
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<LocationManager>();

// Setting up Identity 
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager<SignInManager<ApplicationUser>>();

// Configure application cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// DbInitializer
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

// Repository Structure Implementation
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();

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

// External Services
// Cloudinary
builder.Services.AddSingleton<CloudinaryService>();

// GiaoHangNhanh
builder.Services.AddHttpClient<GhnService>();

var app = builder.Build();

// Seed the database
SeedDatabase();

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

app.MapHub<JobHub>("/jobHub");

app.Run();

async Task SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        await dbInitializer.Initialize();
    }
}