// Copyright (c) Peter Vrenken. All rights reserved. See the license on https://github.com/vrenken/EtAlii.Trends

namespace EtAlii.Trends;

using System;
using System.Diagnostics;
using System.Threading.Tasks;

public static class ObservableSubscribeAsyncExtension
{
    public static IDisposable SubscribeAsync<T>(
        this IObservable<T> source,
        Func<T,Task> onNext,
        Action<Exception>? onError = null,
        Action? onCompleted = null)
    {
        void OnNext(T o)
        {
            try
            {
                var task = Task.Run(() => onNext(o));
                task.GetAwaiter().GetResult();
            }
            catch
            {
                if(Debugger.IsAttached)
                {
                    //throw;
                }
                else
                {
                    //Let's do nothing and just experience the exceptions when a debugger is attached.
                }
            }
        }

        if (onError != null && onCompleted != null)
        {
            return source.Subscribe(OnNext, onError, onCompleted);
        }
        if (onCompleted != null)
        {
            return source.Subscribe(OnNext, onCompleted);
        }
        return onError != null
            ? source.Subscribe(OnNext, onError)
            : source.Subscribe(OnNext);
    }
}
