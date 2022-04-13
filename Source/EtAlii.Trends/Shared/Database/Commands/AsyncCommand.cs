namespace EtAlii.Trends
{
    using System;

    /// <summary>
    /// use this base class to define async commands that do not return a result.
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public abstract class AsyncCommand<THandler> : AsyncCommand
        where THandler : IAsyncCommandHandler
    {
        protected AsyncCommand() : base(typeof(THandler))
        {
        }
    }

    /// <summary>
    /// use this base class to define async commands that do return a result.
    /// </summary>
    /// <typeparam name="THandler"></typeparam>
    public abstract class AsyncCommandWithResult<THandler> : AsyncCommandWithResult
        where THandler : IAsyncCommandHandlerWithResult
    {
        protected AsyncCommandWithResult() : base(typeof(THandler))
        {
        }
    }

    /// <summary>
    /// Do not use this base class - it should only be used by the dispatcher itself.
    /// </summary>
    public abstract class AsyncCommandWithResult : AsyncCommand
    {
        protected AsyncCommandWithResult(Type handlerType) : base(handlerType)
        {
        }
    }
    /// <summary>
    /// Do not use this base class - it should only be used by the dispatcher itself.
    /// </summary>
    public abstract class AsyncCommand
    {
        public Type HandlerType { get; }

        protected AsyncCommand(Type handlerType)
        {
            HandlerType = handlerType;
        }
    }
}
