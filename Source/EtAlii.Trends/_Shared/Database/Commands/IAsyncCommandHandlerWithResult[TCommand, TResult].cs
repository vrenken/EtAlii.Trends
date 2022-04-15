namespace EtAlii.Trends
{
    using System.Threading.Tasks;

    /// <summary>
    /// Use this interface to define handlers able to process commands that return a result asynchronously.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface IAsyncCommandHandlerWithResult<in TCommand, TResult> : IAsyncCommandHandlerWithResult
        where TCommand: AsyncCommand
    {
        Task<object> IAsyncCommandHandlerWithResult.Handle(AsyncCommand command)
        {
            var task = Task.Run(async () => await Handle((TCommand)command).ConfigureAwait(false));
            var result = task.GetAwaiter().GetResult()!;
            return Task.FromResult<object>(result);
        }
        Task<TResult> Handle(TCommand command);
    }
}
