namespace EtAlii.Trends
{
    /// <summary>
    /// Use this interface to define handlers able to process commands synchronously.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandHandler<in TCommand> : ICommandHandler
        where TCommand: Command
    {
        void ICommandHandler.Handle(Command command) => Handle((TCommand)command);
        void Handle(TCommand command);
    }
}
