namespace EtAlii.Trends
{
    /// <summary>
    /// Do not use this interface - it should only be used by the dispatcher itself.
    /// </summary>
    public interface ICommandHandlerWithResult
    {
        object Handle(Command command);
    }
}
