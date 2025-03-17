namespace WorkNestify.DataAccess.DbInitializer;

public interface IDbInitializer
{
    void Initialize();
    void SeedEntities();
}