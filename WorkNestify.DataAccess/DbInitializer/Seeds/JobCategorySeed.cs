using WorkNestify.Models.Models.Jobs;

namespace WorkNestify.DataAccess.DbInitializer.Seeds;

public static class JobCategorySeed
{
    public static List<JobCategory> GetJobCategories()
    {
        return new List<JobCategory>
        {
            new JobCategory
            {
                Name = "Information Technology",
                Description = "Software development, cybersecurity, networking, and IT support"
            },
            new JobCategory
            {
                Name = "Marketing", 
                Description = "Digital marketing, SEO, content creation, and branding"
            },
            new JobCategory
            {
                Name = "Sales",
                Description = "Business development, B2B/B2C sales, and customer relationship management"
            },
            new JobCategory
            {
                Name = "Healthcare",
                Description = "Medical professionals, nursing, pharmaceuticals, and hospital administration"
            },
            new JobCategory
            {
                Name = "Finance & Accounting",
                Description = "Accounting, auditing, financial analysis, and banking"
            },
            new JobCategory
            {
                Name = "Human Resources", 
                Description = "Recruitment, employee relations, and HR management"
            },
            new JobCategory
            {
                Name = "Engineering", 
                Description = "Mechanical, electrical, civil, and software engineering"
            },
            new JobCategory
            {
                Name = "Education & Training", 
                Description = "Teaching, tutoring, and corporate training"
            },
            new JobCategory
            {
                Name = "Customer Service", 
                Description = "Call center, technical support, and client relations"
            },
            new JobCategory
            {
                Name = "Logistics & Supply Chain",
                Description = "Transportation, inventory management, and procurement"
            }
        };
    }
}