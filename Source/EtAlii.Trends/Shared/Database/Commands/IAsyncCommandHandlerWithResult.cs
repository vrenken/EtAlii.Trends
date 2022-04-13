namespace EtAlii.Trends
{
    using System.Threading.Tasks;

    /// <summary>
    /// Do not use this interface - it should only be used by the dispatcher itself.
    /// </summary>
    public interface IAsyncCommandHandlerWithResult
    {
        Task<object> Handle(AsyncCommand command);
    }
}
