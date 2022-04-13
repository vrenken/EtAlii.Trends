namespace EtAlii.Trends
{
    using System.Threading.Tasks;

    /// <summary>
    /// Do not use this interface - it should only be used by the dispatcher itself.
    /// </summary>
    public interface IAsyncCommandHandler
    {
        Task Handle(AsyncCommand command);
    }
}
