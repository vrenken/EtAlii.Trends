namespace EtAlii.Trends;

public class DatabaseInitializer
{
    public void InitializeWhenNeeded()
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        data.Database.EnsureCreated();

        if (!data.Users.Any())
        {
            // Initialize the database.
            var user = new User { Name = "test", Password = "123", };
            var diagram = new Diagram { Name = "Test diagram", User = user };
            data.Users.Add(user);
            data.Diagrams.Add(diagram);

            var layer1 = new Layer { Name = "Hierarchical layers", Expanded = true, Diagram = diagram};
            data.Add(layer1);

            var layer2 = new Layer { Parent = layer1, Name = "Business", Diagram = diagram };
            data.Add(layer2);

            var layer3 = new Layer { Parent = layer1, Name = "Functional", Diagram = diagram };
            data.Add(layer3);

            var layer4 = new Layer { Parent = layer1, Name = "User interface", Diagram = diagram };
            data.Add(layer4);

            var layer5 = new Layer { Parent = layer1, Name = "Infrastructure", Diagram = diagram };
            data.Add(layer5);

            var layer6 = new Layer { Parent = layer1, Name = "Storage", Diagram = diagram };
            data.Add(layer6);

            var layer7 = new Layer { Parent = layer1, Name = "Sensors", Diagram = diagram };
            data.Add(layer7);

            var layer14 = new Layer { Name = "Global trends", Expanded = true, Diagram = diagram };
            data.Add(layer14);

            var layer15 = new Layer { Name = "Data meshing", Parent = layer14, Diagram = diagram };
            data.Add(layer15);

            var layer16 = new Layer { Name = "Head Mounted Displays", Parent = layer14, Diagram = diagram };
            data.Add(layer16);

            var layer17 = new Layer { Name = "Data Ecosystem Platforms", Parent = layer14, Diagram = diagram };
            data.Add(layer17);

            data.SaveChanges();
        }
    }
}
