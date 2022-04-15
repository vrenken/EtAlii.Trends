namespace EtAlii.Trends
{
    using System.Threading.Tasks;

    /// <summary>
    /// Use this interface to define handlers able to process commands asynchronously.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface IAsyncCommandHandler<in TCommand> : IAsyncCommandHandler
        where TCommand: AsyncCommand
    {
        Task IAsyncCommandHandler.Handle(AsyncCommand command) => Handle((TCommand)command);
        Task Handle(TCommand command);
    }
}
