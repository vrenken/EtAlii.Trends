namespace EtAlii.Trends;

using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

public class ThrottledInvocation : IDisposable
{
    private readonly Subject<int> _subject;
    private readonly IDisposable _subscription;

    public ThrottledInvocation(TimeSpan throttle, Func<Task> invocation)
    {
        _subject = new Subject<int>();

        _subscription = _subject
            .Throttle(throttle, TaskPoolScheduler.Default)
            .SubscribeAsync(async _ => await invocation().ConfigureAwait(false));
    }

    public void Raise()
    {
        _subject.OnNext(0);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _subscription.Dispose();
        _subject.Dispose();
    }
}
