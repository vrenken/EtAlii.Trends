namespace EtAlii.Trends
{
    using System;

    /// <summary>
    /// use this base class to define async commands that do not return a result.
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public abstract record AsyncCommand<THandler>() : AsyncCommand(typeof(THandler)) where THandler : IAsyncCommandHandler;

    /// <summary>
    /// use this base class to define async commands that do return a result.
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public abstract record AsyncCommandWithResult<THandler>() : AsyncCommandWithResult(typeof(THandler)) where THandler : IAsyncCommandHandlerWithResult;

    /// <summary>
    /// Do not use this base class - it should only be used by the dispatcher itself.
    /// </summary>
    public abstract record AsyncCommandWithResult(Type HandlerType) : AsyncCommand(HandlerType);

    /// <summary>
    /// Do not use this base class - it should only be used by the dispatcher itself.
    /// </summary>
    public abstract record AsyncCommand(Type HandlerType);
}
