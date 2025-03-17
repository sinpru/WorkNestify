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
                Id = 1, Name = "Information Technology",
                Description = "Software development, cybersecurity, networking, and IT support"
            },
            new JobCategory
            {
                Id = 2, Name = "Marketing", Description = "Digital marketing, SEO, content creation, and branding"
            },
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
            {
                Id = 8, Name = "Education & Training", Description = "Teaching, tutoring, and corporate training"
            },
            new JobCategory
            {
                Id = 9, Name = "Customer Service", Description = "Call center, technical support, and client relations"
            },
            new JobCategory
            {
                Id = 10, Name = "Logistics & Supply Chain",
                Description = "Transportation, inventory management, and procurement"
            }
        };
    }
}