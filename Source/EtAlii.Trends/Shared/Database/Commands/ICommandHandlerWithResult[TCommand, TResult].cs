namespace EtAlii.Trends
{
    /// <summary>
    /// Use this interface to define handlers able to process commands that return a result synchronously.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public interface ICommandHandlerWithResult<in TCommand, out TResult> : ICommandHandlerWithResult
        where TCommand: Command
    {
        object ICommandHandlerWithResult.Handle(Command command) => Handle((TCommand)command)!;
        TResult Handle(TCommand command);
    }
}
