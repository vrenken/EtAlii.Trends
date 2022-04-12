namespace EtAlii.Trends;

public class Entity
{
    /// <summary>
    /// This is the identifier that we use to identify each Entity in the EF Core
    /// code/datastore with.
    /// </summary>
    public Guid Id { get; init; }
    //public Guid Id { get; protected set; } // TODO: Revert to protected Id.
}
