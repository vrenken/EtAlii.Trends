namespace EtAlii.Trends;

public class DatabaseInitializer
{
    public void InitializeWhenNeeded()
    {
        using var data = new DataContext();
        data.Database.EnsureCreated();

        if (!data.Users.Any())
        {
            // Initialize the database.
            var user = new User { Name = "test", Password = "123", };
            var diagram = new Diagram { Name = "Test diagram", User = user };
            data.Users.Add(user);
            data.Diagrams.Add(diagram);
            data.SaveChanges();
        }
    }
}
