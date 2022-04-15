namespace EtAlii.Trends
{
    using System.Threading.Tasks;

    public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatch a non-async command, without returning any results.
        /// </summary>
        /// <param name="command"></param>
        void Dispatch(Command command);

        /// <summary>
        /// Dispatch a non-async command that returns a result.
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        TResult Dispatch<TResult>(CommandWithResult command);

        /// <summary>
        /// Dispatch an async command, without returning any results.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        Task DispatchAsync(AsyncCommand command);

        /// <summary>
        /// Dispatch a sequence of async commands, without returning any results.
        /// </summary>
        /// <returns></returns>
        Task DispatchAsync(params AsyncCommand[] commands);

        /// <summary>
        /// Dispatch an async command that returns a result.
        /// </summary>
        /// <param name="command"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        Task<TResult> DispatchAsync<TResult>(AsyncCommandWithResult command);
    }
}
