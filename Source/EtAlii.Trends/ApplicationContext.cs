namespace EtAlii.Trends;

public class ApplicationContext
{
    public void Initialize()
    {
        // ReSharper disable once UseAwaitUsing
        using var data = new DataContext();

        // Do stuff.
    }
}
