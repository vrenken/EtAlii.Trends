namespace EtAlii.Trends
{
    using System;

    /// <summary>
    /// use this base class to define non-async commands that do not return a result.
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public abstract class Command<THandler> : Command
        where THandler : ICommandHandler
    {
        protected Command() : base(typeof(THandler))
        {
        }
    }

    /// <summary>
    /// use this base class to define non-async commands that do return a result.
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public abstract class CommandWithResult<THandler> : CommandWithResult
        where THandler : ICommandHandlerWithResult
    {
        protected CommandWithResult() : base(typeof(THandler))
        {
        }
    }

    /// <summary>
    /// Do not use this base class - it should only be used by the dispatcher itself.
    /// </summary>
    public abstract class CommandWithResult : Command
    {
        protected CommandWithResult(Type handlerType) : base(handlerType)
        {
        }
    }

    /// <summary>
    /// Do not use this base class - it should only be used by the dispatcher itself.
    /// </summary>
    public abstract class Command
    {
        public Type HandlerType { get; }

        protected Command(Type handlerType)
        {
            HandlerType = handlerType;
        }
    }
}
