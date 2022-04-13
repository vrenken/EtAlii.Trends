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

        public CommandDispatcher(IServiceProvider serviceProvider) // Func<Type, object> commandHandlerResolver
        {
            _commandHandlerResolver = type => serviceProvider.GetService(type)!; // commandHandlerResolver;
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
                var handler = (ICommandHandler)_commandHandlerResolver(command.HandlerType);
                handler.Handle(command);
            }
            finally
            {
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
                var handler = (ICommandHandlerWithResult)_commandHandlerResolver(command.HandlerType);
                result = (TResult)handler.Handle(command);
            }
            finally
            {
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
                var handler = (IAsyncCommandHandler)_commandHandlerResolver(command.HandlerType);
                await handler
                    .Handle(command)
                    .ConfigureAwait(false);
            }
            finally
            {
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
                var handler = (IAsyncCommandHandlerWithResult)_commandHandlerResolver(command.HandlerType);
                result = (TResult)await handler
                    .Handle(command)
                    .ConfigureAwait(false);

            }
            finally
            {
                _lock.Release();
            }

            _log.Debug("Handled command {CommandName}", command.GetType().Name);

            return result;
        }
    }
}
