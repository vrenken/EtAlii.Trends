namespace EtAlii.Trends;

using EtAlii.Trends.Diagrams;
using EtAlii.Trends.Editor.Layers;
using EtAlii.Trends.Editor.Trends;

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

            var diagram = new Diagram
            {
                Name = "Test diagram",
                User = user,
            };
            diagram.HorizontalOffset = Diagram.StartOffset;
            diagram.VerticalOffset = Diagram.StartOffset;
            diagram.DiagramZoom = 1D;

            data.Users.Add(user);
            data.Diagrams.Add(diagram);
            data.Entry(user).State = EntityState.Added;
            data.Entry(diagram).State = EntityState.Added;

            var layer1 = new Layer { Name = "Hierarchical layers", IsExpanded = true, Diagram = diagram};
            data.Add(layer1);
            data.Entry(layer1).State = EntityState.Added;

            var layer2 = new Layer { Parent = layer1, Name = "Business", Diagram = diagram };
            data.Add(layer2);
            data.Entry(layer2).State = EntityState.Added;

            var layer3 = new Layer { Parent = layer1, Name = "Functional", Diagram = diagram };
            data.Add(layer3);
            data.Entry(layer3).State = EntityState.Added;

            var layer4 = new Layer { Parent = layer1, Name = "User interface", Diagram = diagram };
            data.Add(layer4);
            data.Entry(layer4).State = EntityState.Added;

            var layer5 = new Layer { Parent = layer1, Name = "Infrastructure", Diagram = diagram };
            data.Add(layer5);
            data.Entry(layer5).State = EntityState.Added;

            var layer6 = new Layer { Parent = layer1, Name = "Storage", Diagram = diagram };
            data.Add(layer6);
            data.Entry(layer6).State = EntityState.Added;

            var layer7 = new Layer { Parent = layer1, Name = "Sensors", Diagram = diagram };
            data.Add(layer7);
            data.Entry(layer7).State = EntityState.Added;

            var layer14 = new Layer { Name = "Global trends", IsExpanded = true, Diagram = diagram };
            data.Add(layer14);
            data.Entry(layer14).State = EntityState.Added;

            var layer15 = new Layer { Name = "Data meshing", Parent = layer14, Diagram = diagram };
            data.Add(layer15);
            data.Entry(layer15).State = EntityState.Added;

            var layer16 = new Layer { Name = "Head Mounted Displays", Parent = layer14, Diagram = diagram };
            data.Add(layer16);
            data.Entry(layer16).State = EntityState.Added;

            var layer17 = new Layer { Name = "Data Ecosystem Platforms", Parent = layer14, Diagram = diagram };
            data.Add(layer17);
            data.Entry(layer17).State = EntityState.Added;

            // Trends
            var trends = new List<Trend>
            {
                new()
                {
                    Name = "Internet",
                    Diagram = diagram,
                    Begin = new DateTime(1960, 1, 1),
                    End = new DateTime(2023, 1, 1),
                    Components = new []
                    {
                        new Component
                        {
                            Name = "DARPA",
                            Moment = new DateTime(1960, 1, 1),
                        },
                        new Component
                        {
                            Name = "Now",
                            Moment = new DateTime(2023, 1, 1)
                        }
                    }
                },
                new()
                {
                    Name = "Mobile phones",
                    Diagram = diagram,
                    Begin = new DateTime(1992, 1, 1),
                    End = new DateTime(2014, 1, 1),
                    Components = new []
                    {
                        new Component
                        {
                            Name = "Motorola 8900X-2",
                            Moment = new DateTime(1992, 1, 1),
                        },
                        new Component
                        {
                            Name = "iPhone",
                            Moment = new DateTime(2014, 1, 1)
                        }
                    }
                },
                new()
                {
                    Name = "Metaverse",
                    Diagram = diagram,
                    Begin = new DateTime(2021, 1, 1),
                    End = new DateTime(2035, 1, 1),
                    Components = new []
                    {
                        new Component
                        {
                            Name = "Facebook announcement",
                            Moment = new DateTime(2021, 1, 1),
                        },
                        new Component
                        {
                            Name = "Microsoft announcement",
                            Moment = new DateTime(2022, 1, 1)
                        },
                        new Component
                        {
                        Name = "Apple announcement",
                        Moment = new DateTime(2023, 1, 1)
                        }
                    }
                },
            };

            var horizontalPosition = Diagram.StartOffset;
            var verticalPosition = Diagram.StartOffset;

            foreach (var trend in trends)
            {
                trend.X = horizontalPosition += 100D;
                trend.Y = verticalPosition += 100D;
                trend.W = 150;
                trend.H = 50;

                data.Trends.Add(trend);
                data.Entry(trend).State = EntityState.Added;
            }

            data.Entry(diagram).State = EntityState.Added;

            foreach (var component in trends.SelectMany(t => t.Components))
            {
                data.Components.Add(component);
                data.Entry(component).State = EntityState.Added;
            }

            data.SaveChanges();
        }
    }
}
