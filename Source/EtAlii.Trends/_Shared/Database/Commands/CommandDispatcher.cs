namespace EtAlii.Trends
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Serilog;

    /// <inheritdoc />
    public class CommandDispatcher : ICommandDispatcher
    {
        private const int DebugLockTimeout = 10000;
        private readonly Func<Type, object> _commandHandlerResolver;
        private readonly SemaphoreSlim _lock = new(1, 1);

        private readonly ILogger _log = Log.ForContext<CommandDispatcher>();

        public event Action Started = () => { };
        public event Action Stopped = () => { };

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _commandHandlerResolver = type =>
            {
                var handler = serviceProvider.GetService(type);
                if (handler == null)
                {
                    _log.Fatal("Unable to find command handler for {HandlerType}", type.Name);
                }

                return handler!;
            };
        }

        /// <inheritdoc />
        public void Dispatch(Command command)
        {
            _log.Debug("Handling command {CommandName}", command.GetType().Name);

#if DEBUG
            if (!_lock.Wait(DebugLockTimeout))
            {
                _log.Error("Command dispatcher locked on executing command {CommandName}", command.GetType().Name);
                _lock.Wait();
            }
#else
            _lock.Wait();
#endif
            try
            {
                Started.Invoke();
                var handler = (ICommandHandler)_commandHandlerResolver(command.HandlerType);
                handler.Handle(command);
            }
            finally
            {
                Stopped.Invoke();
                _lock.Release();
            }

            _log.Debug("Handled command {CommandName}", command.GetType().Name);
        }

        /// <inheritdoc />
        public TResult Dispatch<TResult>(CommandWithResult command)
        {
            _log.Debug("Handling command {CommandName}", command.GetType().Name);

            TResult result;

#if DEBUG
            if (!_lock.Wait(DebugLockTimeout))
            {
                _log.Error("Command dispatcher locked on executing command {CommandName}", command.GetType().Name);
                _lock.Wait();
            }
#else
            _lock.Wait();
#endif
            try
            {
                Started.Invoke();
                var handler = (ICommandHandlerWithResult)_commandHandlerResolver(command.HandlerType);
                result = (TResult)handler.Handle(command);
            }
            finally
            {
                Stopped.Invoke();
                _lock.Release();
            }

            _log.Debug("Handled command {CommandName}", command.GetType().Name);

            return result;
        }

        /// <inheritdoc />
        public async Task DispatchAsync(AsyncCommand command)
        {
            _log.Debug("Handling command {CommandName}", command.GetType().Name);

#if DEBUG
            var success = await _lock.WaitAsync(DebugLockTimeout).ConfigureAwait(false);
            if (!success)
            {
                _log.Error("Command dispatcher locked on executing command {CommandName}", command.GetType().Name);
                await _lock.WaitAsync().ConfigureAwait(false);
            }
#else
            await _lock.WaitAsync();
#endif
            try
            {
                Started.Invoke();
                var handler = (IAsyncCommandHandler)_commandHandlerResolver(command.HandlerType);
                await handler
                    .Handle(command)
                    .ConfigureAwait(false);
            }
            finally
            {
                Stopped.Invoke();
                _lock.Release();
            }

            _log.Debug("Handled command {CommandName}", command.GetType().Name);
        }


        /// <inheritdoc />
        public async Task DispatchAsync(params AsyncCommand[] commands)
        {
            _log
                .ForContext("CommandNames", commands.Select(c => c.GetType().Name).ToArray(), true)
                .Debug("Handling commands");

#if DEBUG
            var success = await _lock.WaitAsync(DebugLockTimeout).ConfigureAwait(false);
            if (!success)
            {
                _log
                    .ForContext("CommandNames", commands.Select(c => c.GetType().Name).ToArray(), true)
                    .Error("Command dispatcher locked on executing commands");
                await _lock.WaitAsync().ConfigureAwait(false);
            }
#else
            await _lock.WaitAsync();
#endif
            try
            {
                Started.Invoke();
                foreach (var command in commands)
                {
                    var handler = (IAsyncCommandHandler)_commandHandlerResolver(command.HandlerType);
                    await handler
                        .Handle(command)
                        .ConfigureAwait(false);
                }
            }
            finally
            {
                Stopped.Invoke();
                _lock.Release();
            }

            _log
                .ForContext("CommandNames", commands.Select(c => c.GetType().Name).ToArray(), true)
                .Verbose("Handled commands");
        }


        /// <inheritdoc />
        public async Task<TResult> DispatchAsync<TResult>(AsyncCommandWithResult command)
        {
            _log.Debug("Handling command {CommandName}", command.GetType().Name);

            TResult result;
#if DEBUG
            var success = await _lock.WaitAsync(DebugLockTimeout).ConfigureAwait(false);
            if (!success)
            {
                _log.Error("Command dispatcher locked on executing command {CommandName}", command.GetType().Name);
                await _lock.WaitAsync().ConfigureAwait(false);
            }
#else
            await _lock.WaitAsync();
#endif
            try
            {
                Started.Invoke();
                var handler = (IAsyncCommandHandlerWithResult)_commandHandlerResolver(command.HandlerType);
                result = (TResult)await handler
                    .Handle(command)
                    .ConfigureAwait(false);

            }
            finally
            {
                Stopped.Invoke();
                _lock.Release();
            }

            _log.Debug("Handled command {CommandName}", command.GetType().Name);

            return result;
        }
    }
}
