using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkNestify.Models.Models.Companies;
using WorkNestify.Models.Models.JobApplications;
using WorkNestify.Models.Models.Jobs;
using WorkNestify.Models.Models.Locations;
using WorkNestify.Models.Models.Users;
using WorkNestify.Utilities.Constants;

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

    // Locations
    public DbSet<Ward> Wards { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Province> Provinces { get; set; }

    // Users
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Data for JobCategory
        modelBuilder.Entity<JobCategory>().HasData(
            new JobCategory
            {
                Id = 1, Name = "Information Technology",
                Description = "Software development, cybersecurity, networking, and IT support"
            },
            new JobCategory
                { Id = 2, Name = "Marketing", Description = "Digital marketing, SEO, content creation, and branding" },
            new JobCategory
            {
                Id = 3, Name = "Sales",
                Description = "Business development, B2B/B2C sales, and customer relationship management"
            },
            new JobCategory
            {
                Id = 4, Name = "Healthcare",
                Description = "Medical professionals, nursing, pharmaceuticals, and hospital administration"
            },
            new JobCategory
            {
                Id = 5, Name = "Finance & Accounting",
                Description = "Accounting, auditing, financial analysis, and banking"
            },
            new JobCategory
            {
                Id = 6, Name = "Human Resources", Description = "Recruitment, employee relations, and HR management"
            },
            new JobCategory
            {
                Id = 7, Name = "Engineering", Description = "Mechanical, electrical, civil, and software engineering"
            },
            new JobCategory
                { Id = 8, Name = "Education & Training", Description = "Teaching, tutoring, and corporate training" },
            new JobCategory
            {
                Id = 9, Name = "Customer Service", Description = "Call center, technical support, and client relations"
            },
            new JobCategory
            {
                Id = 10, Name = "Logistics & Supply Chain",
                Description = "Transportation, inventory management, and procurement"
            });

        // Seed Data for Province
        modelBuilder.Entity<Province>().HasData(
            new Province { Id = 201, Name = "Hà Nội" },
            new Province { Id = 202, Name = "Hồ Chí Minh" },
            new Province { Id = 203, Name = "Đà Nẵng" });

        // Seed Data for District
        modelBuilder.Entity<District>().HasData(
            new District { Id = 1488, Name = "Quận Hai Bà Trưng", ProvinceId = 201 },
            new District { Id = 1489, Name = "Quận Hoàn Kiếm", ProvinceId = 201 },
            new District { Id = 1486, Name = "Quận Đống Đa", ProvinceId = 201 },
            new District { Id = 1485, Name = "Quận Cầu Giấy", ProvinceId = 201 },
            new District { Id = 1493, Name = "Quận Thanh Xuân", ProvinceId = 201 },
            new District { Id = 1455, Name = "Quận Tân Bình", ProvinceId = 202 },
            new District { Id = 1444, Name = "Quận 3", ProvinceId = 202 },
            new District { Id = 1442, Name = "Quận 1", ProvinceId = 202 },
            new District { Id = 1446, Name = "Quận 4", ProvinceId = 202 },
            new District { Id = 1462, Name = "Quận Bình Thạnh", ProvinceId = 202 });

        modelBuilder.Entity<Ward>().HasData(
            new Ward { Code = "1A0602", Name = "Phường Dịch Vọng Hậu", DistrictId = 1485 },
            new Ward { Code = "1A0706", Name = "Phường Nhân Chính", DistrictId = 1493 },
            new Ward { Code = "21402", Name = "Phường 2", DistrictId = 1455 },
            new Ward { Code = "20304", Name = "Phường 4", DistrictId = 1444 },
            new Ward { Code = "20109", Name = "Phường Phạm Ngũ Lão", DistrictId = 1442 },
            new Ward { Code = "1A0407", Name = "Phường Láng Thượng", DistrictId = 1486 });
        
        // Seed Data for Company
        modelBuilder.Entity<Company>().HasData(
            new Company
            {
                Id = 1,
                Name = "FPT Corporation",
                Website = "https://fpt.com/en",
                Email = "ir@fpt.com",
                Phone = "+84 24 7300 7300",
                StreetAddress = "FPT Tower, 10 Pham Van Bach Street",
                Description = "<h1>About FPT Corporation</h1>  \n\n<h2>Overview</h2>  \n<p>FPT Corporation is a leading technology and IT services company based in Vietnam. Established in 1988, it has grown into one of the most influential technology firms in the country.</p>  \n\n<h2>Core Services</h2>  \n<p>FPT specializes in various technology sectors, including software development, digital transformation, telecommunications, and education. The company provides innovative solutions to businesses worldwide, helping them enhance efficiency and competitiveness.</p>  \n\n<h2>Industry Presence</h2>  \n<p>With a strong presence in industries such as finance, healthcare, manufacturing, and retail, FPT continuously supports enterprises in modernizing their operations through advanced digital solutions.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>FPT invests heavily in cutting-edge technologies like artificial intelligence (AI), cloud computing, big data, and the Internet of Things (IoT) to drive digital transformation and industry disruption.</p>  \n\n<h2>Global Expansion</h2>  \n<p>Operating in over 25 countries, FPT is committed to expanding its global footprint. By partnering with major international firms, it delivers high-quality technology services to businesses worldwide.</p>  \n\n<h2>Commitment to the Future</h2>  \n<p>FPT remains dedicated to innovation, talent development, and technological advancement, shaping the future of digital transformation and contributing to the global tech ecosystem.</p>  \n",
                Logo = "https://upload.wikimedia.org/wikipedia/commons/1/11/FPT_logo_2010.svg",
                Industry = "Information technology, telecommunications, education",
                FoundedDate = new DateTime(1988, 09, 13),
                Size = CompanySizes.Large,
                ProvinceId = 201,
                DistrictId = 1485,
                WardCode = "1A0602"
            },
            new Company
            {
                Id = 2,
                Name = "Base.vn",
                Website = "https://base.vn",
                Email = "contact@base.vn",
                Phone = "+84 24 2244 1313",
                StreetAddress = "123 Nguyen Trai Street",
                Description = "<h1>About Base.vn</h1>  \n\n<h2>Overview</h2>  \n<p>Base.vn is a leading SaaS company in Vietnam, offering digital transformation solutions to help enterprises optimize their operations and improve efficiency.</p>  \n\n<h2>Core Services</h2>  \n<p>Base.vn provides a wide range of cloud-based software solutions, including workflow automation, human resource management, business intelligence, and collaboration tools.</p>  \n\n<h2>Industry Presence</h2>  \n<p>Trusted by thousands of businesses across various industries, Base.vn empowers organizations to streamline processes, enhance productivity, and drive digital innovation.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>With a strong focus on technology, Base.vn leverages AI, big data, and automation to develop intelligent enterprise solutions that cater to modern business needs.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Base.vn continues to expand its ecosystem of products and services, supporting businesses in their digital transformation journey across Vietnam and beyond.</p>  \n\n<h2>Commitment to Businesses</h2>  \n<p>Dedicated to helping companies succeed in the digital age, Base.vn remains committed to innovation, customer success, and delivering cutting-edge SaaS solutions.</p>  \n",
                Logo = "https://static-gcdn.basecdn.net/landing/base.vn/image/v2/logo/base.png",
                Industry = "Software as a Service (SaaS)",
                FoundedDate = new DateTime(2016, 8, 1),
                Size = CompanySizes.Small,
                ProvinceId = 201,
                DistrictId = 1493,
                WardCode = "1A0706"
            },
            new Company
            {
                Id = 3,
                Name = "KiotViet",
                Website = "https://www.kiotviet.vn",
                Email = "hotro@kiotviet.vn",
                Phone = "+84 86 2533 433",
                StreetAddress = "C Section, Waseco Building, 10 Pho Quang Street",
                Description = "<h1>About KiotViet</h1>  \n\n<h2>Overview</h2>  \n<p>KiotViet provides cloud-based POS and business management software tailored for small businesses, helping them streamline operations and improve efficiency.</p>  \n\n<h2>Core Services</h2>  \n<p>KiotViet offers a comprehensive suite of solutions, including point-of-sale (POS) systems, inventory management, customer relationship management (CRM), and sales tracking.</p>  \n\n<h2>Industry Presence</h2>  \n<p>Serving over 150,000 businesses across various sectors, KiotViet is a trusted solution for retailers, wholesalers, and service providers looking to digitize their operations.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>KiotViet leverages cloud computing, data analytics, and automation to provide scalable and user-friendly business management tools that enhance decision-making.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>With a rapidly growing user base, KiotViet continues to expand its features and services, ensuring businesses have access to the latest digital solutions for growth.</p>  \n\n<h2>Commitment to Small Businesses</h2>  \n<p>KiotViet is dedicated to empowering small and medium-sized enterprises (SMEs) with affordable, efficient, and easy-to-use software, supporting their success in the digital economy.</p>  \n",
                Logo = "https://logo.kiotviet.vn/KiotViet-Logo-Horizontal.svg",
                Industry = "Retail Technology, POS Systems",
                FoundedDate = new DateTime(2014, 5, 15),
                Size = CompanySizes.Small,
                ProvinceId = 202,
                DistrictId = 1455,
                WardCode = "21402"
            },
            new Company
            {
                Id = 4,
                Name = "The Coffee House",
                Website = "https://www.thecoffeehouse.com",
                Email = "hi@thecoffeehouse.com",
                Phone = "+84 28 7107 8079",
                StreetAddress = "86 - 88 Cao Thang Street",
                Description = "<h1>About The Coffee House</h1>  \n\n<h2>Overview</h2>  \n<p>The Coffee House is a fast-growing Vietnamese coffee chain known for its modern, cozy atmosphere and high-quality beverages.</p>  \n\n<h2>Signature Offerings</h2>  \n<p>The Coffee House serves a diverse menu of specialty coffee, tea, and pastries, crafted to suit the tastes of Vietnamese coffee lovers.</p>  \n\n<h2>Store Experience</h2>  \n<p>Designed as a welcoming space for work, study, and social gatherings, each store provides a comfortable ambiance with contemporary decor and friendly service.</p>  \n\n<h2>Commitment to Quality</h2>  \n<p>With a focus on sustainability, The Coffee House sources high-quality coffee beans from local farmers, ensuring fresh and flavorful drinks in every cup.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Since its launch, The Coffee House has rapidly expanded across Vietnam, becoming a favorite destination for coffee enthusiasts and casual customers alike.</p>  \n\n<h2>Community Engagement</h2>  \n<p>Beyond serving coffee, The Coffee House fosters a strong community culture, encouraging creativity, connections, and shared experiences in every store.</p>  \n",
                Logo = "https://upload.wikimedia.org/wikipedia/commons/9/97/The_Coffee_House_logo.svg",
                Industry = "Food & Beverage, Coffee Retail",
                FoundedDate = new DateTime(2014, 7, 1),
                Size = CompanySizes.Medium,
                ProvinceId = 202,
                DistrictId = 1444,
                WardCode = "20304"
            },
            new Company
            {
                Id = 5,
                Name = "Foody.vn",
                Website = "https://www.foody.vn",
                Email = "support@shopeefood.vn",
                Phone = "+84 28 7300 2200",
                StreetAddress = "Jabes 1 Building, 244 Cong Quynh Street",
                Description = "<h1>About Foody.vn</h1>  \n\n<h2>Overview</h2>  \n<p>Foody.vn is a leading food discovery, restaurant review, and delivery platform in Vietnam, connecting users with thousands of dining options across the country.</p>  \n\n<h2>Core Services</h2>  \n<p>Foody.vn offers restaurant reviews, user ratings, and a seamless food delivery service, helping customers find and order from their favorite eateries.</p>  \n\n<h2>Industry Presence</h2>  \n<p>With a vast database of restaurants, cafes, and street food vendors, Foody.vn serves as a go-to platform for food enthusiasts looking for new culinary experiences.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Foody.vn leverages data-driven recommendations, AI-powered search, and an intuitive mobile app to enhance the food discovery and ordering experience.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>Since its launch, Foody.vn has expanded its services nationwide, partnering with restaurants and delivery providers to bring convenience to millions of users.</p>  \n\n<h2>Commitment to Users</h2>  \n<p>Foody.vn is dedicated to making dining more accessible and enjoyable by continuously improving its platform and providing reliable, user-generated reviews.</p>  \n",
                Logo = "https://www.foody.vn/style/images/logo/foody-vn.png",
                Industry = "Food Delivery, E-commerce",
                FoundedDate = new DateTime(2012, 6, 5),
                Size = CompanySizes.Medium,
                ProvinceId = 202,
                DistrictId = 1442,
                WardCode = "20109"
            },
            new Company
            {
                Id = 6,
                Name = "Digiworld",
                Website = "https://digiworld.com.vn",
                Email = "contact@digiworld.com.vn",
                Phone = "+84 28 3802 8700",
                StreetAddress = "14th Floor, TNR Tower, 54A Nguyen Chi Thanh Street",
                Description = "<h1>About Digiworld</h1>  \n\n<h2>Overview</h2>  \n<p>Digiworld is a leading distributor of technology products, including laptops, smartphones, and accessories, serving as a key bridge between global brands and the Vietnamese market.</p>  \n\n<h2>Core Services</h2>  \n<p>Digiworld specializes in product distribution, market expansion, logistics, and after-sales services, ensuring seamless operations for its partners and customers.</p>  \n\n<h2>Industry Presence</h2>  \n<p>As a trusted distributor, Digiworld collaborates with top international tech brands, bringing high-quality products to consumers and businesses across Vietnam.</p>  \n\n<h2>Technology & Innovation</h2>  \n<p>Leveraging advanced supply chain management and digital transformation, Digiworld optimizes distribution channels to enhance efficiency and market reach.</p>  \n\n<h2>Growth & Expansion</h2>  \n<p>With a strong nationwide network, Digiworld continues to expand its portfolio, covering diverse technology segments such as computing, mobility, and smart devices.</p>  \n\n<h2>Commitment to Excellence</h2>  \n<p>Digiworld is committed to delivering top-tier products and services, supporting brand growth, and enriching Vietnam’s technology landscape.</p>  \n",
                Logo = "https://digiworld.com.vn/assets/site/homes/logo/Artboard-1.png",
                Industry = "Technology, Distribution",
                FoundedDate = new DateTime(1997, 9, 1),
                Size = CompanySizes.Large,
                ProvinceId = 201,
                DistrictId = 1486,
                WardCode = "1A0407"
            });

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

        // Locations Relationships
        modelBuilder.Entity<Ward>()
            .HasOne(w => w.District)
            .WithMany(d => d.Wards)
            .HasForeignKey(w => w.DistrictId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}