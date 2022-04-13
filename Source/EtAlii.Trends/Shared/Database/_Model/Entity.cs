namespace EtAlii.Trends;

public class Entity
{
    /// <summary>
    /// This is the identifier that we use to identify each Entity in the EF Core
    /// code/datastore with.
    /// </summary>
#pragma warning disable CS8618
    public Guid Id { get; protected set; }
#pragma warning restore CS8618
}
