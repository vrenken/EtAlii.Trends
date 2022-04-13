namespace EtAlii.Trends
{
    using System;

    /// <summary>
    /// use this base class to define non-async commands that do not return a result.
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public abstract record Command<THandler>() : Command(typeof(THandler)) where THandler : ICommandHandler;

    /// <summary>
    /// use this base class to define non-async commands that do return a result.
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public abstract record CommandWithResult<THandler>() : CommandWithResult(typeof(THandler)) where THandler : ICommandHandlerWithResult;

    /// <summary>
    /// Do not use this base class - it should only be used by the dispatcher itself.
    /// </summary>
    public abstract record CommandWithResult(Type HandlerType) : Command(HandlerType);

    /// <summary>
    /// Do not use this base class - it should only be used by the dispatcher itself.
    /// </summary>
    public abstract record Command(Type HandlerType);
}
