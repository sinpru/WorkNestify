namespace WorkNestify.DataAccess.DbInitializer;

public interface IDbInitializer
{
    Task Initialize();
    Task SeedEntities();
}